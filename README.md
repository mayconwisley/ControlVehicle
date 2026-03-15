# ControleVeiculo

API REST para gestão de frota e operação de veículos, com controle de:
- Motoristas
- Veículos
- Saídas/retornos (`VehicleControl`)
- Abastecimentos (`FuelControl`)
- Multas (`TrafficFineControl`)
- Manutenções (`MaintenanceControl`)

A solução segue arquitetura em camadas com separação entre domínio, aplicação, infraestrutura e API.

## Sumário
- [Visão Geral](#visao-geral)
- [Arquitetura da Solução](#arquitetura-da-solucao)
- [Stack Tecnológica](#stack-tecnologica)
- [Pré-requisitos](#pre-requisitos)
- [Configuração do Ambiente](#configuracao-do-ambiente)
- [Execução da API](#execucao-da-api)
- [Migrações e Banco de Dados](#migracoes-e-banco-de-dados)
- [Documentação da API](#documentacao-da-api)
- [Recursos e Endpoints (v1)](#recursos-e-endpoints-v1)
- [Exemplos de Payload](#exemplos-de-payload)
- [Paginação e Busca](#paginacao-e-busca)
- [Regras de Domínio Importantes](#regras-de-dominio-importantes)
- [Testes](#testes)
- [Estrutura de Pastas](#estrutura-de-pastas)
- [Troubleshooting](#troubleshooting)

## Visao Geral
O projeto expõe uma API versionada (`v1`) com persistência em PostgreSQL (EF Core + Npgsql).

A aplicação foi modelada para manter regras de negócio no domínio (value objects, entidades e validações) e usar a camada de aplicação para orquestrar casos de uso, mapeamento DTO <-> entidade e persistência via repositórios.

## Arquitetura da Solucao
A solução (`ControlVehicle.sln`) é composta por:

- `ControlVehicle.Api`
  - Camada de apresentação (ASP.NET Core).
  - Controllers HTTP, versionamento de API e documentação OpenAPI/Scalar.

- `ControlVehicle.App`
  - Camada de aplicação.
  - Serviços de caso de uso (`DriverServices`, `VehicleServices`, etc.).

- `ControlVehicle.Domain`
  - Núcleo do domínio.
  - Entidades, enums, value objects, contratos de repositório e paginação.

- `ControlVehicle.Infra`
  - Implementação de infraestrutura.
  - `DbContext`, mapeamentos EF Core, repositórios, Unit of Work e migrations.

- `ControlVehicle.Models`
  - DTOs e mapeamentos para transporte de dados.

- `ControlVehicle.Tests`
  - Testes de unidade das camadas de aplicação e domínio.

## Stack Tecnologica
- .NET SDK: `10.0.200`
- ASP.NET Core: `net10.0`
- Entity Framework Core: `10.0.3`
- Banco: PostgreSQL (`Npgsql.EntityFrameworkCore.PostgreSQL 10.0.0`)
- Versionamento da API: `Asp.Versioning.Mvc 8.1.0`
- OpenAPI + UI: `Microsoft.AspNetCore.OpenApi` + `Scalar.AspNetCore`
- Testes: `xUnit`, `Microsoft.NET.Test.Sdk`, `coverlet.collector`

## Pre-requisitos
- .NET SDK 10
- PostgreSQL em execução (padrão local: `localhost:5432`)
- Permissão para criar banco/schema e aplicar migrations

## Configuracao do Ambiente
### 1) Connection string
Arquivo: `ControlVehicle.Api/appsettings.json`

```json
{
  "ConnectionStrings": {
    "VehicleConnection": "Host=localhost;Port=5432;Database=control_vehicle;Username=postgres;Password="
  }
}
```

Observação: em runtime, a senha **não** é lida do JSON. A API injeta a senha a partir da variável de ambiente `SQLPassword`.

### 2) Variável obrigatória da API
A API valida `SQLPassword` no escopo de ambiente de máquina (`EnvironmentVariableTarget.Machine`).

PowerShell (executar como administrador):

```powershell
[System.Environment]::SetEnvironmentVariable('SQLPassword', 'SUA_SENHA_AQUI', 'Machine')
```

Feche e abra o terminal após definir a variável.

### 3) Variável opcional para tooling EF
Para comandos de design-time do EF (`dotnet ef`), é possível usar:

- `EF_CONNECTION_STRING` (prioritária), ou
- `SQLPassword` para montar a string local padrão.

Exemplo:

```powershell
$env:EF_CONNECTION_STRING = 'Host=localhost;Port=5432;Database=control_vehicle;Username=postgres;Password=SUA_SENHA_AQUI'
```

## Execucao da API
Na raiz da solução:

```powershell
dotnet restore ControlVehicle.sln
dotnet build ControlVehicle.sln -c Release
dotnet run --project .\ControlVehicle.Api\ControlVehicle.Api.csproj
```

URLs locais (perfil `https`):
- `https://localhost:7096`
- `http://localhost:5027`

## Migracoes e Banco de Dados
As migrations estão em `ControlVehicle.Infra/Database/Migrations`.

Aplicar migrations:

```powershell
dotnet ef database update --project .\ControlVehicle.Infra\ControlVehicle.Infra.csproj --startup-project .\ControlVehicle.Api\ControlVehicle.Api.csproj
```

Criar nova migration:

```powershell
dotnet ef migrations add NomeDaMigration --project .\ControlVehicle.Infra\ControlVehicle.Infra.csproj --startup-project .\ControlVehicle.Api\ControlVehicle.Api.csproj --output-dir Database\Migrations
```

Schema padrão usado pelo `DbContext`: `control_vehicle`.

## Documentacao da API
Em ambiente `Development`, a API publica:

- OpenAPI JSON: `/doc/v1.json`
- UI Scalar: `/doc/scalar`

Exemplo local:
- [https://localhost:7096/doc/scalar](https://localhost:7096/doc/scalar)

## Recursos e Endpoints (v1)
Base route: `/api/v1`

### Driver
- `GET /Driver?page=1&size=5&search=`
- `GET /Driver/{cnh}`
- `POST /Driver`
- `PUT /Driver/{id:guid}`
- `DELETE /Driver/{cnh}`

### Vehicle
- `GET /Vehicle?page=1&size=5&search=`
- `GET /Vehicle/Plate/{plate}`
- `GET /Vehicle/Renavam/{renavam}`
- `POST /Vehicle`
- `PUT /Vehicle/{id:guid}`
- `DELETE /Vehicle/{renavam}`

### VehicleControl
- `GET /VehicleControl?page=1&size=10&search=`
- `GET /VehicleControl/{id:guid}`
- `POST /VehicleControl`
- `PUT /VehicleControl/{id:guid}`
- `DELETE /VehicleControl/{id:guid}`

### FuelControl
- `GET /FuelControl?page=1&size=5&search=`
- `GET /FuelControl/{id:guid}`
- `POST /FuelControl`
- `PUT /FuelControl/{id:guid}`
- `DELETE /FuelControl/{id:guid}`

### TrafficFineControl
- `GET /TrafficFineControl?page=1&size=5&search=`
- `GET /TrafficFineControl/{id:guid}`
- `POST /TrafficFineControl`
- `PUT /TrafficFineControl/{id:guid}`
- `DELETE /TrafficFineControl/{id:guid}`

### MaintenanceControl
- `GET /MaintenanceControl?page=1&size=5&search=`
- `GET /MaintenanceControl/{id:guid}`
- `POST /MaintenanceControl`
- `PUT /MaintenanceControl/{id:guid}`
- `DELETE /MaintenanceControl/{id:guid}`

## Exemplos de Payload
A API serializa enums como string (`JsonStringEnumConverter`).

### Criar motorista
```json
{
  "name": "Maria Silva",
  "cnh": "12345678901",
  "categoryCnh": "B",
  "dateExpiration": "2028-12-31",
  "active": true
}
```

### Criar veículo
```json
{
  "renavam": "12345678901",
  "model": "Onix",
  "licensePlate": "ABC1D23",
  "fuel": "Gasoline",
  "chassi": "9BWZZZ377VT004251",
  "vehicleColor": "Black",
  "active": true
}
```

### Criar controle de uso do veículo
```json
{
  "vehicleId": "00000000-0000-0000-0000-000000000000",
  "driverId": "00000000-0000-0000-0000-000000000000",
  "departureDate": "2026-03-15T08:00:00Z",
  "arrivalDate": "2026-03-15T18:00:00Z",
  "initialKm": 10000,
  "finalKm": 10120,
  "description": "Visita técnica em clientes"
}
```

### Criar abastecimento
```json
{
  "vehicleId": "00000000-0000-0000-0000-000000000000",
  "driverId": "00000000-0000-0000-0000-000000000000",
  "initialKm": 10120,
  "value": 250.75,
  "date": "2026-03-15T10:30:00Z",
  "liters": 42.3,
  "description": "Posto BR"
}
```

### Criar multa
```json
{
  "vehicleId": "00000000-0000-0000-0000-000000000000",
  "driverId": "00000000-0000-0000-0000-000000000000",
  "points": 4,
  "value": 130.16,
  "date": "2026-03-10T14:00:00Z",
  "description": "Excesso de velocidade"
}
```

### Criar manutenção
```json
{
  "vehicleId": "00000000-0000-0000-0000-000000000000",
  "date": "2026-03-01T09:00:00Z",
  "value": 890.00,
  "description": "Troca de pneus"
}
```

## Paginacao e Busca
Nos endpoints de listagem:
- `page`: página atual (mínimo efetivo = 1)
- `size`: tamanho da página (mínimo efetivo = 5, exceto `VehicleControl` com default 10)
- `search`: filtro textual/por identificadores conforme recurso

Resposta de listagem:

```json
{
  "totalData": 100,
  "page": 1,
  "totalPage": 20,
  "size": 5,
  "vehicleList": []
}
```

Se não houver itens, os controllers retornam `404 NotFound`.

## Regras de Dominio Importantes
- `Cnh`: obrigatório, apenas dígitos, 11 caracteres.
- `Renavam`: obrigatório, apenas dígitos, 11 caracteres.
- `LicensePlate`: aceita padrão antigo (`ABC1234`) e Mercosul (`ABC1D23`).
- `Chassi`: VIN com 17 caracteres alfanuméricos, sem `I`, `O` e `Q`.
- `CategoryCnh`: aceita `A`, `B`, `C`, `D`, `E`, `AB`, `AC`, `AD`, `AE`.
- `VehicleControl`: `arrivalDate >= departureDate`; `finalKm >= initialKm`; descrição obrigatória e até 1000 chars.
- `FuelControl`: litros > 0; valor e km não negativos; descrição opcional até 500 chars.
- `TrafficFineControl`: pontos e valor não negativos; descrição opcional até 500 chars.
- `MaintenanceControl`: valor não negativo; descrição opcional até 500 chars.

Enums principais:
- `FuelEnum`: `Gasoline`, `Diesel`, `Ethanol`, `Flex`, `Hybrid`, `Electric`, `Hydrogen`, `Cng`, `Lpg`.
- `VehicleColorEnum`: `White`, `Black`, `Gray`, `Silver`, `Blue`, `Red`, `Brown`, `Green`, `Beige`.

## Testes
Executar todos os testes:

```powershell
dotnet test ControlVehicle.sln -c Release
```

Cobertura atual inclui:
- Serviços de aplicação para todos os recursos principais.
- Value objects (`Cnh`, `LicensePlate`).

## Estrutura de Pastas
```text
ControlVehicle/
  ControlVehicle.Api/
  ControlVehicle.App/
  ControlVehicle.Domain/
  ControlVehicle.Infra/
  ControlVehicle.Models/
  ControlVehicle.Tests/
  ControlVehicle.sln
```

## Troubleshooting
- Erro: `Variavel de ambiente 'SQLPassword' nao encontrada (Machine).`
  - Defina a variável `SQLPassword` no escopo `Machine` e reabra o terminal.

- Erro de conexão com PostgreSQL.
  - Verifique host/porta/banco/usuário em `appsettings.json`.
  - Confirme se a senha em `SQLPassword` está correta.
  - Confirme se o PostgreSQL está ativo.

- `dotnet ef` não encontra contexto.
  - Use os parâmetros `--project` e `--startup-project` conforme seção de migrações.
  - Defina `EF_CONNECTION_STRING` para evitar ambiguidades de ambiente.

---

Projeto licenciado sob [MIT](LICENSE.txt).

## Frontend (React + MUI)

Foi adicionado o projeto `ControlVehicle.Frontend` com:
- React + TypeScript (Vite)
- MUI com layout responsivo
- Dark mode com persistência em `localStorage`
- Telas de listagem por recurso consumindo a API `/api/v1`

Execução:

```powershell
cd .\ControlVehicle.Frontend
copy .env.example .env
npm install
npm run dev
```
