﻿using BusinessObjects;
using DataAccessLayer.DTO;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public class BookingHistoryDAO
{
    public static async Task<BookingReservation?> GetBookingById(int id)
    {
        using var db = new FuminiHotelManagementContext();
        return await db.BookingReservations.FirstOrDefaultAsync(b => b.Equals(id));
    }

    public static async Task<List<BookingHistoryDTO>> GetBookingByCusId(int id)
    {
        using var db = new FuminiHotelManagementContext();
        return await db.BookingDetails
            .Include(bd => bd.BookingReservation)
                .ThenInclude(br => br.Customer)
            .Include(bd => bd.Room)
            .Where(bd => bd.BookingReservation.CustomerId == id)
            .Select(bd => new BookingHistoryDTO
            {
                BookingReservationId = bd.BookingReservationId,
                RoomId = bd.RoomId,
                RoomNumber = bd.Room.RoomNumber,
                StartDate = bd.StartDate,
                EndDate = bd.EndDate,
                ActualPrice = bd.ActualPrice,
                BookingDate = bd.BookingReservation.BookingDate,
                TotalPrice = bd.BookingReservation.TotalPrice,
                CustomerId = bd.BookingReservation.CustomerId,
                BookingStatus = bd.BookingReservation.BookingStatus
            })
            .ToListAsync();
    }
}
