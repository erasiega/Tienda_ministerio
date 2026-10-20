namespace Tienda.Pruebas.Falsos;

/// <summary>
/// Pone el Id a una entidad, como lo haría la base de datos al guardarla.
/// Las entidades tienen el Id con set privado, así que se usa reflexión solo en las pruebas.
/// </summary>
internal static class Ids
{
    public static void Asignar(object entidad, int id) =>
        entidad.GetType().GetProperty("Id")!.SetValue(entidad, id);
}
