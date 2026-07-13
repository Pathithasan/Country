using System;

namespace Country.Api.Models
{
	public class CountryModel
	{
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string CountryCode { get; set; } = string.Empty;

        public string? Capital { get; set; }

        public string? Region { get; set; }

        public long Population { get; set; } = 0;

        public decimal Area { get; set; } = 0;

        public string Flag { get; set; } = string.Empty;

        public string? FlagUrl { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

    }
}