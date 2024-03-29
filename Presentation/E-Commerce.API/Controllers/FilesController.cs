﻿using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FilesController : ControllerBase
	{
		private readonly IConfiguration _configuration;

		public FilesController(IConfiguration configuration)
		{
			_configuration = configuration;
		}


		[HttpGet("[action]")]
		public IActionResult GetBaseStorageUrl()
		{

			return Ok(new { baseStorageUrl = !string.IsNullOrEmpty(_configuration["StorageBaseUrl"]) ? _configuration["StorageBaseUrl"] : "" });
		}
	}
}
