# 📦 DGI_PruebaTecnica (.NET por capas)

Proyecto de ejemplo con **ASP.NET Core Web API**, **Entity Framework Core**, **AutoMapper**, **Unit of Work + Repositorio Genérico** y pruebas con **MSTest**.

---

## 🧩 Arquitectura

```
DGI_PruebaTecnica.sln
├─ DGI_PruebaTecnica.Api/                 # Capa API (endpoints, Swagger)
├─ DGI_PruebaTecnica.Aplicacion/          # Capa de aplicación (DTOs, Servicios, Profiles AutoMapper)
│  ├─ Commons/                            # Clases base, Ordering, Responses, etc.
│  ├─ Dtos/
│  ├─ Entities/
│  ├─ Helpers/
│  └─ Services/
├─ DGI_PruebaTecnica.Infraestructura/     # Capa de infraestructura (DbContext, Repos, UoW)
│  ├─ Persistences/Context/
│  ├─ Persistences/Interfaces/
│  └─ Persistences/Repositories/
└─ DGI_PruebaTecnica.Test/                # Pruebas unitarias MSTest con InMemory
   ├─ Infrastructure/
   ├─ ComprobantesAppService_MSTest.cs
   └─ ContribuyentesAppService_MSTest.cs
```

---

## ⚙️ Configuración

### `appsettings.json` (API)
```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost,1433;Database=DGI_PruebaTecnica;User Id=sa;Password=TuPass!;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Migraciones con EF Core
```bash
# crear migración
dotnet ef migrations add InitialCreate --project DGI_PruebaTecnica.Infraestructura --startup-project DGI_PruebaTecnica.Api

# aplicar migración
dotnet ef database update --project DGI_PruebaTecnica.Infraestructura --startup-project DGI_PruebaTecnica.Api
```

---

## ▶️ Ejecutar la API

```bash
dotnet run --project DGI_PruebaTecnica.Api
```

Swagger estará disponible en:  
👉 http://localhost:5000/swagger (o el puerto configurado).

---

## ✅ Pruebas Unitarias

- Framework: **MSTest**
- Base de datos: **EF Core InMemory**
- Helpers reales: `OrderingQuery`, `DateRangeHelper`
- AutoMapper configurado con `LoggerFactory`

Ejecutar todos los tests:

```bash
dotnet test
```

Ejecutar solo los de un servicio específico:

```bash
dotnet test --filter FullyQualifiedName~ComprobantesAppService_MSTest
dotnet test --filter FullyQualifiedName~ContribuyentesAppService_MSTest
```

---

## 🔌 Servicios principales

### ContribuyentesAppService
- **ListarAsync(FilterContribuyenteRequest)**  
  Filtros: texto (`NumFilter`), `StateFilter`, `Estatus` por nombre, rango de fechas (`StartDate`/`EndDate`), orden y paginación.
- **GetByIdAsync(int id)**  
  Incluye navegación a `TipoContribuyente` y `EstatusContribuyente`.

### ComprobantesAppService
- **ListarAsync(FilterContribuyenteComprobanteRequest)**  
  Filtros: RNC, texto (NCF/RNC), fechas de emisión, orden y paginación.
- **ObtenerPorContribuyenteAsync(ComprobantesPorContribuyenteRequest)**  
  Valida RNC, filtra por fechas, calcula total ITBIS, proyecta a DTO de listado, ordena/pagina.

---

## 🧪 Tips de testing

- Los **unit tests** se ejecutan contra **InMemory**, nunca contra SQL real.  
- Si necesitas sembrar entidades en tests, completa todos los campos requeridos (`Codigo` en `TipoNCF`, etc.).  
- Para columnas con `private set` (`Itbis18`), se setean con **ChangeTracker** en el seed de pruebas.  
- Si el `OrderingQuery` no respeta `DESC`, valida con campos deterministas (`Id`).

---

## 🧰 Comandos útiles

```bash
# restaurar paquetes
dotnet restore

# compilar
dotnet build -c Release

# ejecutar API
dotnet run --project DGI_PruebaTecnica.Api
```

---

## 📝 Licencia

Uso educativo y demostrativo. Ajusta a tus necesidades.
