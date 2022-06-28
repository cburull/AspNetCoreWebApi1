using AspNetCoreWebApi1.Model;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AspNetCoreWebApi1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        private string connStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=db0001;Integrated Security=True;";


        [HttpGet("{id}")]
        public async Task<Person> GetPerson(int id)
        {
            var query = $"select * from People where Id = {id}";
            var connection = new SqlConnection(connStr);
            return await connection.QueryFirstOrDefaultAsync<Person>(query);
        }


        [HttpGet]
        public async Task<List<Person>> GetPeople()
        {
            var query = $"select * from People";
            var connection = new SqlConnection(connStr);
            return (List<Person>)await connection.QueryAsync<Person>(query);
        }


        [HttpPost]
        public async Task<List<Person>> Add(Person p)
        {
            List<Person> people = new List<Person>();
            var task = @$"insert into People (FirstName, LastName, Age) 
                       values ('{p.FirstName}', '{p.LastName}', {p.Age})";

            await DB(task);
            return await GetPeople();
        }


        [HttpPatch]
        public async Task<List<Person>> Edit(Person p)
        {
            List<Person> people = new List<Person>();
            var task = @$"update People set FirstName='{p.FirstName}', 
                       LastName='{p.LastName}', Age={p.Age} where Id={p.Id}";

            await DB(task);
            return await GetPeople();
        }


        [HttpDelete("{id}")]
        public async Task<List<Person>> Delete(int id)
        {
            var task = $"delete from People where Id={id}";
            await DB(task);
            return await GetPeople();
        }


        public async Task DB(string task)
        {
            var connection = new SqlConnection(connStr);
            int i = await connection.ExecuteAsync(task);
        }
    }
}