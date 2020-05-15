using leave_management.Contracts;
using leave_management.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Repository
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveRequestRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool Create(LeaveRequest entity)
        {
            _db.LeaveRequests.Add(entity);
            return Save();
        }

        public bool Delete(LeaveRequest entity)
        {
            _db.LeaveRequests.Remove(entity);
            return Save();
        }

        public ICollection<LeaveRequest> FindAll()
        {
            return _db.LeaveRequests
                .Include(s => s.ApprovedBy)
                .Include(s => s.RequestingEmployee)
                .Include(s => s.LeaveType)
                .ToList();
        }

        public LeaveRequest FindById(int id)
        {
           var leaveHistory =  _db.LeaveRequests
                .Include(s => s.ApprovedBy)
                .Include(s => s.RequestingEmployee)
                .Include(s => s.LeaveType)
                .FirstOrDefault(s => s.Id == id);
            return leaveHistory;
        }

        public bool Save()
        {
            var result = _db.SaveChanges();
            return result > 0;
        }

        public bool Update(LeaveRequest entity)
        {
            _db.LeaveRequests.Update(entity);
            return Save();
        }

        public bool IsExists(int id)
        {
            var result = _db.LeaveRequests.Any(s => s.Id == id);
            return result;
        }

        public ICollection<LeaveRequest> GetLeaveRequestsByEmployee(string employeeid)
        {
            var leaveRequests =  FindAll();
            return leaveRequests.Where(q => q.RequestingEmployeeId == employeeid)
            .ToList();
        }
    }
}
