namespace Tienda.Pruebas.Falsos;

/// <summary>Un reloj que siempre da la misma fecha, para que las pruebas no dependan del día en que corren.</summary>
internal sealed class RelojFijo : TimeProvider
{
    private readonly DateTimeOffset _ahora;

    public RelojFijo(DateTimeOffset ahora) => _ahora = ahora;

    public override DateTimeOffset GetUtcNow() => _ahora.ToUniversalTime();

    public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.Utc;
}
