using System;

namespace Country.Api.ViewModels
{
	public class CountryVM
	{
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? CountryCode { get; set; } 

        public string? Capital { get; set; }

        public string? Region { get; set; }

        public long Population { get; set; }

        public decimal Area { get; set; }

        public string? Flag { get; set; }

        public string? FlagUrl { get; set; }

    }
}