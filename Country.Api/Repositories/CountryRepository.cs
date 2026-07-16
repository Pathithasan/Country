using Microsoft.Data.SqlClient;
using System.Data;
using Country.Api.Models;  

namespace Country.Api.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CountryRepository> _logger;

        public CountryRepository(
            IConfiguration configuration,
            ILogger<CountryRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
        
        public async Task<List<CountryModel>> GetAllAsync()
        {
            try
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
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while retrieving all countries.");

                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid database operation while retrieving all countries.");

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while retrieving all countries.");

                throw;
            }
        }

        public async Task<CountryModel?> GetByCodeAsync(string code)
        {
            try
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
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while retrieving country '{CountryCode}'.",code);

                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid database operation while retrieving country '{CountryCode}'.", code);

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while retrieving country '{CountryCode}'.", code);

                throw;
            }    
        }
    
        public async Task AddAsync(CountryModel country)
        {
            try
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
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while adding country '{CountryCode}'.", country.CountryCode);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while adding country '{CountryCode}'.", country.CountryCode);
                throw;
            }
        }
    }
}