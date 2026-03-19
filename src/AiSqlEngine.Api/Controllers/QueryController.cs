using AiSqlEngine.Core.Interfaces;
using AiSqlEngine.Core.Models.Request;
using AiSqlEngine.Core.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiSqlEngine.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/query")]
public class QueryController : ControllerBase
{
    private readonly IAiQueryService _aiQueryService;

    public QueryController(IAiQueryService aiQueryService)
    {
        _aiQueryService = aiQueryService;
    }

    [HttpPost("generate-sql")]
    public async Task<ActionResult<GenerateSqlResponse>> GenerateSql([FromBody] GenerateSqlRequest request, CancellationToken cancellationToken)
    {
        var result = await _aiQueryService.ConvertToSqlAsync(request.Message, cancellationToken);

        if (result.IsFailed)
        {
            return BadRequest(new { errors = result.Errors.Select(e => e.Message) });
        }

        return Ok(new GenerateSqlResponse(result.Value));
    }

    [HttpPost("execute")]
    public async Task<IActionResult> ExecuteSql([FromBody] ExecuteSqlRequest request, CancellationToken cancellationToken)
    {
        var result = await _aiQueryService.ExecuteAsync(request.Sql, cancellationToken);

        if (result.IsFailed)
        {
            return BadRequest(new { errors = result.Errors.Select(e => e.Message) });
        }

        return Ok(result.Value);
    }
}