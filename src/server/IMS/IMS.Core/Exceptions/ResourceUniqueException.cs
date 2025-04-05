using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Core.Exceptions
{
	public class ResourceUniqueException(string message) : Exception(message)
	{
	}
}