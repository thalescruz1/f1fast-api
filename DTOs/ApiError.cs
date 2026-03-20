// ============================================================
// DTO: ApiError — Resposta padronizada de erro da API
// ============================================================
// Toda vez que a API retornar um erro (400, 401, 404, 500...),
// o corpo da resposta terá SEMPRE este formato JSON:
//
//   { "status": 400, "mensagem": "Prazo encerrado para esta etapa." }
//
// Isso facilita o tratamento no frontend: ele sempre sabe que
// o campo "mensagem" terá o texto para exibir ao usuário.
// ============================================================

namespace F1Fast.API.DTOs;

public record ApiError(int Status, string Mensagem);
