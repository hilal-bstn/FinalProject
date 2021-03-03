using System;
using System.Collections.Generic;
using System.Text;
namespace Core.Entities.Concrete
{
    public class UserOperationClaim:IEntity

    { 
        public int Id { get; set; }
        public int UserId { get; set; }//Bu UserId ye sahip kişi 
        public int OperationClaimId { get; set; }//bu Operasyonları gerçekleştirebilir.
    }
}
