using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Security.Principal;
using WebAPI.Entity;
using WebAPI.Exception;
using WebAPI.Repository;
using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Services
{
    public class PaymentServiceImpl : GenericServices, IPaymentService
    {
        private readonly ITicketService _ticketService;
        private readonly IInvoiceService _invoiceService;
        private readonly IVnpayService _vnpayService;
        private readonly MovieTheaterContext _movieTheaterContext;
        private IDbContextTransaction _transaction;
        private readonly IQRCodeServicee _qrCodeServicee;
        public PaymentServiceImpl(IMapper mapper, IUnitOfWork unitOfWork,
            ITicketService ticketService, IInvoiceService invoiceService,
            IVnpayService vnpayService, MovieTheaterContext movieTheaterContext,
            IQRCodeServicee qrCodeServicee)
            : base(mapper, unitOfWork)
        {
            _ticketService = ticketService;
            _invoiceService = invoiceService;
            _vnpayService = vnpayService;
            _movieTheaterContext = movieTheaterContext;
            _qrCodeServicee = qrCodeServicee;
        }
        public async Task<string> CreatePaymentAsync(RequestDTOPayment request, HttpContext context, string username)
        {
            double totalAmount = getTotalAmount(request.ScheduleSeats) - request.UseScore;
            var user = await _context.Accounts.FindByNameAsync(username);
            try
            {
                await BeginTransaction();
                var invoiceId = await _invoiceService.CreateInvoiceAsync(user.Id, request, totalAmount);
                var vnp = new RequestDTOVnpay
                {
                    InvoiceId = invoiceId!,
                    TotalAmount = totalAmount,
                    CreatedDate = DateTime.Now,
                    InvoiceMessage = request.invoiceMessage
                };
                var payment = _vnpayService.CreatePaymentUrl(context, vnp)!;
                await CommitTransaction();
                return payment;
            }
            catch (System.Exception ex)
            {
                await RollbackTransaction();
                throw new System.Exception(ex.Message);
            }

        }
        public async Task<ResponseDTOPayment> ExecutePayment(IQueryCollection collections)
        {
            try
            {
                await BeginTransaction();
                var vnp = _vnpayService.PaymentExcute(collections);
                string UserId =await _invoiceService.GetUserIdByInvoiceId(vnp.InvoiceId);
                var payment = new Payment
                {
                    AccountId = UserId,
                    TotalAmount = vnp.vnp_Amount,
                    PaymentDate = vnp.vnp_PayDate,
                    PaymentMethod = "Payment Online by Vnpay"
                };
                if (!vnp.vnp_ResponseCode.Equals("00"))
                {
                    payment.PaymentStatus = "Cancelled";
                    await _invoiceService.UpdateStatusInvoice(vnp.InvoiceId, "Cancelled");
                    await _context.Payments.AddAsync(payment);
                    await CommitTransaction();
                    return new ResponseDTOPayment
                    {
                        Amount = vnp.vnp_Amount,
                        message = "Transaction is cancelled!!!",
                        InvoiceId = vnp.InvoiceId,
                        Status = "02"
                    };
                }
                else
                {
                    payment.PaymentStatus = "Successfull";
                    await _context.Payments.AddAsync(payment);
                    await _invoiceService.UpdateStatusInvoice(vnp.InvoiceId, "Successfull");                    
                    await _context.CommitAsync();
                    await CommitTransaction();
                    return new ResponseDTOPayment
                    {
                        Amount = vnp.vnp_Amount,
                        message = vnp.message,
                        InvoiceId = vnp.InvoiceId,
                        Status = "00"
                    };
                }
            }
            catch (System.Exception ex)
            {
                await RollbackTransaction();
                throw new System.Exception(ex.Message, ex);
            }
        }
        public async Task<ResponseDTOPaymentEmployee> CreatePaymentForEmployeeAsync(RequestDTOPaymentEmployee request, string username)
        {
            double amount = getTotalAmount(request.ScheduleSeats);
            var employee = await _context.Accounts.FindByNameAsync(username);
            if (request.IsUseScore)
            {
                var user = await _context.Accounts.Users.FirstOrDefaultAsync(u => u.IdentityCard == request.member
                      || u.Id == request.member) ?? throw new NotFoundException("Id user or Identity Card is not exist!!!");
                amount -= request.UseScore;
            }
            var content = new RequestDTOQRCode
            {
                AddInfor = request.invoiceMessage,
                Amount = amount
            };
            var requestPayment = _mapper.Map<RequestDTOPayment>(request);
            ResponseDTOQRCode qr = await _qrCodeServicee.GenerateVietQRCodeAsync(content);
            string InvoiceId = await _invoiceService.CreateInvoiceAsync(employee.Id, requestPayment, amount);
            return new ResponseDTOPaymentEmployee
            {
                InvoiceId = InvoiceId,
                QrCode = qr,
                Status = "Successfull"
            };
        }
        private double getTotalAmount(ICollection<RequestDTOPayment.RequestDTOScheduleSeat> list)
        {
            double amount = 0;
            foreach (var item in list)
            {
                amount += item.Type.Price;
            }
            return amount;
        }
        private async Task BeginTransaction()
        {
            if (_transaction == null)
            {
                _transaction = await _movieTheaterContext.Database.BeginTransactionAsync();
            }
        }
        private async Task CommitTransaction()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        private async Task RollbackTransaction()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }
        private void GetCurrentTransaction()
        {
            if (_transaction == null)
            {
                _transaction = _movieTheaterContext.Database.CurrentTransaction!;
            }
        }
        public async Task<ResponseDTOApi> ComfirmPaymentForEmployee(RequestDTOPaymentEmployee request, string username)
        {
            var employee = await _context.Accounts.FindByNameAsync(username);
            var status = (request.Confirm ? "Successfull" : "Canceled");
            await _invoiceService.UpdateStatusInvoice(request.InvoiceId!, "Successfull");
             if(request.Confirm)
            {
                await _ticketService.CreateTickByEmployeeAsync(request.ScheduleSeats, request.InvoiceId!);
                await _context.Members.UpdateScoreById(request.member!, request.InvoiceId!);
            }
            else
            {
                await _ticketService.CreateTickByEmployeeAsync(request.ScheduleSeats, request.InvoiceId!);
            }
            return new ResponseDTOApi
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                StatusMessage = "Successfull!"
            };
            throw new NotImplementedException();
        }
        public async Task<ResponseDTOApi> GenerateTicket(RequestDTOPayment request, string username)
        {
            var user = await _context.Accounts.FindByNameAsync(username);
            await _ticketService.CreateTicketAsync(user.Id, request);
            await _context.Members.UpdateScoreById(user.Id, request.InvoiceId);
            return new ResponseDTOApi
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                StatusMessage = "Successfull!"
            };
        }
    }
}
