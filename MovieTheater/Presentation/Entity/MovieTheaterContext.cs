using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Entity;

public partial class MovieTheaterContext : IdentityDbContext<ApplicationUser>
{
    public MovieTheaterContext()
    {
    }

    public MovieTheaterContext(DbContextOptions<MovieTheaterContext> options)
        : base(options)
    {
    }
    public virtual DbSet<CinemaRoom> CinemaRooms { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<MovieSchedule> MovieSchedules { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<ScheduleSeat> ScheduleSeats { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TransactionHistory> TransactionHistories { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    public virtual DbSet<TypeSeat> TypeSeats { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("server=localhost;database=Movie_Theater;uid=sa;pwd=ductung1706@;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.HasIndex(e => e.IdentityCard, "UQ__Account__4943C3B4978ACBF2").IsUnique();

            entity.HasIndex(e => e.RefreshToken, "UQ__Account__7FB69BADF64BE80B").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Account__AB6E6164764264D0").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__Account__F3DBC572EC494B7E").IsUnique();

            entity.Property(e => e.AccountId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("account_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasMaxLength(255)
                .HasColumnName("fullname");
            entity.Property(e => e.Gender)
                .HasMaxLength(255)
                .HasColumnName("gender");
            entity.Property(e => e.IdentityCard)
                .HasMaxLength(255)
                .HasColumnName("identity_card");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(255)
                .HasColumnName("phone_number");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(255)
                .HasColumnName("refresh_token");
            entity.Property(e => e.RefreshTokenExpire).HasColumnName("refresh_token_expire");
            entity.Property(e => e.RegisterDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("register_date");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Roles_Account");
        });*/

        modelBuilder.Entity<CinemaRoom>(entity =>
        {
            entity.ToTable("Cinema_Room");

            entity.Property(e => e.CinemaRoomId).HasColumnName("cinema_room_id");
            entity.Property(e => e.CinemeRoomName)
                .HasMaxLength(2555)
                .HasColumnName("cineme_room_name");
            entity.Property(e => e.SeatQuantity).HasColumnName("seat_quantity");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.Property(e => e.EmployeeId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("employee_id");
            entity.HasOne(d => d.Account)
                  .WithMany(p => p.Employees)
                  .HasConstraintName("FK_Roles_Employee")
                  .HasForeignKey(a => a.AccountId);
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.ToTable("Invoice");

            entity.Property(e => e.InvoiceId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("invoice_id");
            entity.Property(e => e.AddScore).HasColumnName("add_score");
            entity.Property(e => e.BookingDate)                
                .HasColumnName("booking_date");
            entity.Property(e => e.MovieName)
                .HasMaxLength(255)
                .HasColumnName("movie_name");
            entity.Property(e => e.ScheduleShow).HasColumnName("schedule_show");
            entity.Property(e => e.ScheduleShowTime)
                .HasMaxLength(255)
                .HasColumnName("schedule_show_time");
            entity.Property(e => e.Seat)
                .HasMaxLength(255)
                .HasColumnName("seat");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TotalMoney)
                .HasColumnType("money")
                .HasColumnName("total_money");
            entity.Property(e => e.cinema_room_name);
            entity.Property(e => e.UseScore).HasColumnName("use_score");
            entity.HasOne(e => e.Account)
                  .WithMany(e => e.Invoices)
                  .HasForeignKey(e => e.AccountId);
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.ToTable("Member");
            entity.Property(e => e.MemberId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("member_id");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.HasOne(member => member.Account).WithMany(a => a.Members)
                    .HasForeignKey(member => member.AccountId);
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("Movie");

            entity.Property(e => e.MovieId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("movie_id");
            entity.Property(e => e.Actor)
                .HasMaxLength(255)
                .HasColumnName("actor");
            entity.Property(e => e.CinemaRoomId).HasColumnName("cinema_room_id");
            entity.Property(e => e.Content)
                .HasMaxLength(255)
                .HasColumnName("content");
            entity.Property(e => e.Director)
                .HasMaxLength(255)
                .HasColumnName("director");
            entity.Property(e => e.Duration)
                .HasMaxLength(255)
                .HasColumnName("duration");
            entity.Property(e => e.FromDate).HasColumnName("from_date");
            entity.Property(e => e.LargeImage)
                .HasMaxLength(255)
                .HasColumnName("large_image");
            entity.Property(e => e.MovieNameEnglish)
                .HasMaxLength(255)
                .HasColumnName("movie_name_english");
            entity.Property(e => e.MovieNameVn)
                .HasMaxLength(255)
                .HasColumnName("movie_name_vn");
            entity.Property(e => e.MovieProductionCompany)
                .HasMaxLength(255)
                .HasColumnName("movie_production_company");
            entity.Property(e => e.SmallImage)
                .HasMaxLength(255)
                .HasColumnName("small_image");
            entity.Property(e => e.ToDate).HasColumnName("to_date");
            entity.Property(e => e.Version)
                .HasMaxLength(255)
                .HasColumnName("version");

            entity.HasOne(d => d.CinemaRoom).WithMany(p => p.Movies)
                .HasForeignKey(d => d.CinemaRoomId)
                .HasConstraintName("FK_Cinema_Room_Movie");

            entity.HasMany(d => d.Types).WithMany(p => p.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "MovieType",
                    r => r.HasOne<Type>().WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Type_Movie"),
                    l => l.HasOne<Movie>().WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Movie_Type"),
                    j =>
                    {
                        j.HasKey("MovieId", "TypeId");
                        j.ToTable("Movie_Type");
                        j.IndexerProperty<string>("MovieId")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("movie_id");
                        j.IndexerProperty<int>("TypeId").HasColumnName("type_id");
                    });
        });

        modelBuilder.Entity<MovieSchedule>(entity =>
        {
            entity.HasKey(e => new { e.MovieId, e.ScheduleId });

            entity.ToTable("Movie_Schedule");

            entity.Property(e => e.MovieId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("movie_id");
            entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
            entity.HasOne(d => d.Movie).WithMany(p => p.MovieSchedules)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Movie_Schedule");

            entity.HasOne(d => d.Schedule).WithMany(p => p.MovieSchedules)
                .HasForeignKey(d => d.ScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_Movie");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK_Payment_Id");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("payment_date");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(255)
                .HasColumnName("payment_method");
            entity.Property(e => e.PaymentStatus).HasColumnName("payment_status");
            entity.Property(e => e.TotalAmount)
                .HasColumnName("total_amount");

            entity.HasOne(d => d.Account).WithMany(p => p.Payments)
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_Account_AccountId");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.ToTable("Promotion");

            entity.Property(e => e.PromotionId).HasColumnName("promotion_id");
            entity.Property(e => e.Detail)
                .HasMaxLength(255)
                .HasColumnName("detail");
            entity.Property(e => e.DiscountLevel).HasColumnName("discount_level");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.ToTable("Schedule");

            entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
            entity.Property(e => e.ScheduleTime)
                .HasMaxLength(255)
                .HasColumnName("schedule_time");
            entity.Property(e => e.MovieScheduleDate)
                            .HasMaxLength(255)
                            .HasColumnName("MovieScheduleDate");

        });

        modelBuilder.Entity<ScheduleSeat>(entity =>
        {
            entity.ToTable("Schedule_Seat");

            entity.Property(e => e.ScheduleSeatId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("schedule_seat_id");
            entity.Property(e => e.MovieId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("movie_id");
            entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
            entity.Property(e => e.SeatColumn)
                .HasMaxLength(255)
                .HasColumnName("seat_column");
            entity.Property(e => e.SeatId).HasColumnName("seat_id");
            entity.Property(e => e.SeatRow).HasColumnName("seat_row");
            entity.Property(e => e.SeatStatus).HasColumnName("seat_status");
            entity.HasOne(d => d.Movie)
                .WithMany(p => p.ScheduleSeats)
                .HasForeignKey(d => d.MovieId)  
                .HasConstraintName("FK_Schedule_Seat_Movie");
            entity.HasOne(d => d.Schedule)
                .WithMany(p => p.ScheduleSeats)
                .HasForeignKey(d => d.ScheduleId)  
                .HasConstraintName("FK_Schedule_Seat_Schedule");
            entity.HasOne(d => d.Seat)
                .WithMany(p => p.ScheduleSeats)
                .HasForeignKey(d => d.SeatId)
                .HasConstraintName("FK_Schedule_Seat_Seat");
            entity.HasOne(e => e.TypeSeat)
                  .WithMany(e => e.ScheduleSeats)
                  .HasForeignKey(e => e.seatType_id)
                  .HasPrincipalKey(e => e.Id)
                  .HasConstraintName("FK_TypeSeat_ScheduleSeat")
                  .OnDelete(DeleteBehavior.SetNull);
            entity.HasMany(e => e.Tickets)
                  .WithOne(e => e.ScheduleSeat)
                  .HasForeignKey(e => e.ScheduleSeatId);
            entity.Property(e => e.ReservedUntil);
            entity.HasOne(e => e.Account)
                  .WithMany(e => e.ScheduleSeats)
                  .HasForeignKey(e => e.AccountId);
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.ToTable("Seat");

            entity.Property(e => e.SeatId).HasColumnName("seat_id");
            entity.Property(e => e.CinemaRoomId).HasColumnName("cinema_room_id");
            entity.Property(e => e.SeatColunm)
                .HasMaxLength(255)
                .HasColumnName("seat_colunm");
            entity.Property(e => e.SeatRow).HasColumnName("seat_row");
            entity.Property(e => e.SeatStatus).HasColumnName("seat_status");
            entity.HasOne(d => d.CinemaRoom).WithMany(p => p.Seats)
                .HasForeignKey(d => d.CinemaRoomId)
                .HasConstraintName("FK_Seat_Cinema_Room");
            entity.HasOne(e => e.TypeSeat)
                  .WithMany(e => e.Seats)
                  .HasForeignKey(e => e.seatType_id)
                  .HasPrincipalKey(e => e.Id)
                  .HasConstraintName("FK_Seat_SeatType");
            
        });
        modelBuilder.Entity<TypeSeat>(entity =>
        {
            entity.ToTable("TypeSeat");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn()
                  .HasColumnName("Id");
            entity.Property(e => e.Name)
                  .HasColumnName("TypeName");
            entity.Property(e => e.price)
                  .HasColumnName("price");
            entity.HasMany(e => e.ScheduleSeats)
                  .WithOne(e => e.TypeSeat)
                  .HasForeignKey(e => e.seatType_id);
            entity.HasMany(e => e.Seats)
                   .WithOne(e => e.TypeSeat)
                   .HasForeignKey(e => e.seatType_id);

        });
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("Ticket");

            entity.Property(e => e.TicketId).HasColumnName("ticket_id");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.TicketType).HasColumnName("ticket_type");
            entity.Property(e => e.schedule_show);
            entity.Property(e => e.schedule_show_time);
            entity.Property(e => e.cinema_room_name);
            entity.Property(e => e.movie_name);
            entity.HasOne(e => e.Account)
                  .WithMany(e => e.Tickets)
                  .HasForeignKey(e => e.AccountId);
            entity.HasOne(e => e.ScheduleSeat)
                  .WithMany(e => e.Tickets)
                  .HasForeignKey(e => e.ScheduleSeatId);

        });

        modelBuilder.Entity<TransactionHistory>(entity =>
        {
            entity.ToTable("TransactionHistory");

            entity.Property(e => e.Notes)
                .HasMaxLength(255)
                .HasColumnName("notes");
            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Payment).WithMany(p => p.TransactionHistories)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransactionHistory_Payment");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.ToTable("Type");

            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.TypeName)
                .HasMaxLength(255)
                .HasColumnName("type_name");
        });
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ApplicationUser>().ToTable("Account");
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.HasMany(e => e.Payments)
                  .WithOne(p => p.Account)
                  .HasForeignKey(e => e.AccountId);
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasMaxLength(255)
                .HasColumnName("fullname");
            entity.Property(e => e.Gender)
                .HasMaxLength(255)
                .HasColumnName("gender");
            entity.Property(e => e.IdentityCard)
                .HasMaxLength(255)
                .HasColumnName("identity_card");
            entity.HasIndex(e => e.IdentityCard).IsUnique(true);
            entity.HasIndex(e => e.UserName).IsUnique(true);
            entity.HasIndex(e => e.NormalizedEmail).IsUnique(true);
            entity.HasIndex(e => e.Email).IsUnique(true);
            entity.HasIndex(e => e.RefreshToken).IsUnique(true);
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(255)
                .HasColumnName("phone_number");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(255)
                .HasColumnName("refresh_token");
            entity.Property(e => e.RefreshTokenExpire).HasColumnName("refresh_token_expire");
            entity.Property(e => e.RegisterDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("register_date");
            entity.HasMany(e => e.Employees)
                  .WithOne(e => e.Account);
            entity.HasMany(e => e.Members)
                   .WithOne(a => a.Account);
            entity.HasMany(e => e.Tickets)
                  .WithOne(e => e.Account)
                  .HasForeignKey(e => e.AccountId);
            entity.HasMany(e => e.Invoices)
                  .WithOne(e => e.Account)
                  .HasForeignKey(e => e.AccountId);
            entity.HasMany(e => e.ScheduleSeats)
                  .WithOne(e => e.Account)
                  .HasForeignKey(e => e.AccountId);

        });
        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
