namespace Business.TempModels
{
    public record GenreStats
    {
        public string Genre { get; init; }

        public int MoviesQuantity { get; init; }

        public int SpectaclesQuantity { get; init; }

        public int TotalQuantity => MoviesQuantity + SpectaclesQuantity;
    }
}
