using Microsoft.Data.SqlClient;
using System.Data;
using Country.Api.Models;  

namespace Country.Api.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IConfiguration _configuration;

        public CountryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<List<CountryModel>> GetAllAsync()
        {
            var countries = new List<CountryModel>();

            const string sql = @"
                SELECT
                    Id,
                    Name,
                    CountryCode,
                    Capital,
                    Region,
                    Population,
                    Area,
                    Flag,
                    FlagUrl,
                    CreatedDate,
                    LastModifiedDate
                FROM Countries
                ORDER BY Name;";

            using var connection = GetConnection();
            using var command = new SqlCommand(sql, connection);

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                countries.Add(new CountryModel
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    CountryCode = reader.GetString(2),
                    Capital = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Region = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Population = reader.GetInt64(5),
                    Area = reader.GetDecimal(6),
                    Flag = reader.GetString(7),
                    FlagUrl = reader.IsDBNull(8) ? null : reader.GetString(8),
                    CreatedDate = reader.GetDateTime(9),
                    LastModifiedDate = reader.GetDateTime(10)
                });
            }

            return countries;
        }

        public async Task<CountryModel?> GetByCodeAsync(string code)
        {
            const string sql = @"
                SELECT
                    Id,
                    Name,
                    CountryCode,
                    Capital,
                    Region,
                    Population,
                    Area,
                    Flag,
                    FlagUrl,
                    CreatedDate,
                    LastModifiedDate
                FROM Countries
                WHERE CountryCode = @CountryCode;";

            using var connection = GetConnection();
            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@CountryCode", code);

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

           
            return new CountryModel
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                CountryCode = reader.GetString(2),
                Capital = reader.IsDBNull(3) ? null : reader.GetString(3),
                Region = reader.IsDBNull(4) ? null : reader.GetString(4),
                Population = reader.GetInt64(5),
                Area = reader.GetDecimal(6),
                Flag = reader.GetString(7),
                FlagUrl = reader.IsDBNull(8) ? null : reader.GetString(8),
                CreatedDate = reader.GetDateTime(9),
                LastModifiedDate = reader.GetDateTime(10)
            };
            
        }

        public async Task AddAsync(CountryModel country)
        {
            const string sql = @"
                INSERT INTO Countries
                (
                    Name,
                    CountryCode,
                    Capital,
                    Region,
                    Population,
                    Area,
                    Flag,
                    FlagUrl
                )
                VALUES
                (
                    @Name,
                    @CountryCode,
                    @Capital,
                    @Region,
                    @Population,
                    @Area,
                    @Flag,
                    @FlagUrl
                );";

            using var connection = GetConnection();
            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Name", country.Name);
            command.Parameters.AddWithValue("@CountryCode", country.CountryCode);
            command.Parameters.AddWithValue("@Capital", (object?)country.Capital ?? DBNull.Value);
            command.Parameters.AddWithValue("@Region", (object?)country.Region ?? DBNull.Value);
            command.Parameters.AddWithValue("@Population", country.Population);
            command.Parameters.AddWithValue("@Area", country.Area);
            command.Parameters.AddWithValue("@Flag", country.Flag);
            command.Parameters.AddWithValue("@FlagUrl", (object?)country.FlagUrl ?? DBNull.Value);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}