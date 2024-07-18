using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
	public interface IBannedAccountService
	{
		Task BanUser(int id, string description);
		Task UnBanUser(int id);
	}
}
