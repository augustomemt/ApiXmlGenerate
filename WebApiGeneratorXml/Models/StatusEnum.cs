using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiGeneratorXml.Models
{
    public enum StatusEnum : int 
	{
			UploadError = 0,
			WaitProcess = 1,
			Processing = 2,
			Sucessfull = 3,
			Fail = 4,
			PartialSucess = 5

	}
}