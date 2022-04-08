using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IPAddresController : ControllerBase
    {
        private IPAddres iPAddres = new IPAddres();
        private readonly ILogger<IPAddresController> _logger;
        SqlConnection conn = Connection.GetConnection();
        public IPAddresController(ILogger<IPAddresController> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        public ActionResult Post()
        {
            try
            {
                string filename = $"{Environment.UserName}.txt";
                System.IO.File.WriteAllText(@$"C:\Users\{Environment.UserName}\Desktop\{Environment.UserName}.txt", Dns.GetHostAddresses(Environment.MachineName)[0].ToString());
                byte[] info = System.IO.File.ReadAllBytes(@$"C:\Users\{Environment.UserName}\Desktop\{Environment.UserName}.txt");

                string query = $"INSERT INTO Files (FileName,IpAddres) VALUES (@FileName,@IpAddres)";
                using (SqlCommand sqlCommand = new SqlCommand(query, conn))
                {
                    try
                    {
                        SqlParameter parameter1 = sqlCommand.Parameters.AddWithValue("@FileName", filename);
                        parameter1.DbType = System.Data.DbType.String;
                        SqlParameter parameter2 = sqlCommand.Parameters.AddWithValue("@IpAddres", info);
                        parameter2.DbType = System.Data.DbType.Binary;

                        sqlCommand.ExecuteNonQuery();
                    }
                    catch (System.Exception)
                    {

                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            return StatusCode(200);
        } 
        [HttpGet]
        public ActionResult Get()
        {
            string name = string.Empty;
            string ip = string.Empty;
            try
            {

                using (SqlCommand command = new SqlCommand($"SELECT * FROM [Files]", conn))
                {
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            if(Environment.UserName + ".txt" == reader["FileName"].ToString())
                            {
                                name = reader["FileName"].ToString();
                                var bytes = reader["IpAddres"];
                                ip = Encoding.Unicode.GetString((byte[])bytes);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            return Content($"File name: {name}\nIP: {ip}");
        }
    }
}
