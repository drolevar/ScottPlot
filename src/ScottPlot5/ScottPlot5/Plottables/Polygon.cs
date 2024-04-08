namespace ScottPlot.Plottables;

/// <summary>
/// A polygon is a collection of X/Y points that are all connected to form a closed shape.
/// Polygons can be optionally filled with a color or a gradient.
/// </summary>
public class Polygon : IPlottable
{
    public static Polygon Empty => new();

    public bool IsEmpty => Coordinates.Length == 0;

    public bool Close { get; set; } = true;

    // TODO: replace with a generic data source
    public Coordinates[] Coordinates { get; private set; } = Array.Empty<Coordinates>();

    public string Label { get; set; } = string.Empty;

    public bool IsVisible { get; set; } = true;

    public LineStyle LineStyle { get; set; } = new() { Width = 0 };
    public FillStyle FillStyle { get; set; } = new() { Color = Colors.LightGray };
    public MarkerStyle MarkerStyle { get; set; } = MarkerStyle.None;

    public int PointCount => Coordinates.Length;

    public IAxes Axes { get; set; } = new Axes();

    private AxisLimits limits;

    public IEnumerable<LegendItem> LegendItems => EnumerableExtensions.One<LegendItem>(
        new LegendItem
        {
            Label = Label,
            Marker = MarkerStyle,
            Line = LineStyle,
        });

    private Polygon()
    {
        Coordinates = Array.Empty<Coordinates>();
    }

    /// <summary>
    /// Creates a new polygon.
    /// </summary>
    /// <param name="coords">The axis dependant vertex coordinates.</param>
    public Polygon(Coordinates[] coords)
    {
        UpdateCoordinates(coords);
    }

    public override string ToString()
    {
        string label = string.IsNullOrWhiteSpace(this.Label) ? "" : $" ({this.Label})";
        return $"PlottablePolygon{label} with {PointCount} points";
    }

    private void UpdateCoordinates(Coordinates[] newCoordinates)
    {
        Coordinates = newCoordinates;

        limits = AxisLimits.NoLimits;
        if (IsEmpty) return;

        double xMin = Coordinates[0].X;
        double xMax = Coordinates[0].X;
        double yMin = Coordinates[0].Y;
        double yMax = Coordinates[0].Y;

        foreach (var coord in Coordinates)
        {
            if (coord.X > xMax) xMax = coord.X;
            if (coord.X < xMin) xMin = coord.X;
            if (coord.Y > yMax) yMax = coord.Y;
            if (coord.Y < yMin) yMin = coord.Y;
        }

        limits = new AxisLimits(xMin, xMax, yMin, yMax);
    }

    public AxisLimits GetAxisLimits()
    {
        return limits;
    }

    public void Render(RenderPack rp)
    {
        if (IsEmpty)
            return;

        var coordinates = Close
            ? Coordinates.Concat(new[] { Coordinates.First() })
            : Coordinates;
        
        IEnumerable<Pixel> pixels = coordinates.Select(Axes.GetPixel);

        using var paint = new SKPaint();
        if (Close && FillStyle.HasValue)
        {
            FillStyle.ApplyToPaint(paint, Axes.GetPixelRect(limits.Rect));
            Drawing.DrawLines(rp.Canvas, paint, pixels);
        }

        Drawing.DrawLines(rp.Canvas, paint, pixels, LineStyle);

        if (MarkerStyle.IsVisible)
        {
            Drawing.DrawMarkers(rp.Canvas, paint, pixels, MarkerStyle);
        }
    }
}
