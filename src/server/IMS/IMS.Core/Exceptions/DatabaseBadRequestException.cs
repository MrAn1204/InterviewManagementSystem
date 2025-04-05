using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Core.Exceptions
{
	public class DatabaseBadRequestException(string message) : Exception(message)
	{
	}
}