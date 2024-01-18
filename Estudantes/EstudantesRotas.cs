using ApiCrud.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Estudantes
{
    public static class EstudantesRotas
    {
        public static void AddRotasEstudantes(this WebApplication app)  
        {
            var rotasEstudantes = app.MapGroup("estudantes");

            //app.MapGet("estudantes", () => new Estudante("Joice")); Antes de definir o MapGroup

            rotasEstudantes.MapPost("", async (AddEstudanteRequest request, AppDbContext context, CancellationToken cancellationtoken) =>
            {
                //cancellationtoken enviar no final do metodos async do context, para caso a aplicação cancele a request, o banco tbm cancele a operação
                var jaExiste = await context.Estudantes
                .AnyAsync(estudante => estudante.Nome == request.Nome, cancellationtoken);

                if (jaExiste)
                    return Results.Conflict("Já existe!");



                var novoEstudante = new Estudante(request.Nome);

                await context.Estudantes.AddAsync(novoEstudante);

                await context.SaveChangesAsync(); // Só salva na hora que chamamos o SaveChanges

                return Results.Ok(novoEstudante);
            });

            rotasEstudantes.MapGet("", async (AppDbContext context, CancellationToken cancellationtoken) => {
            var estudantes = await context
                .Estudantes
                .Where(e => e.Ativo)
                .Select(e => new EstudanteDTO(e.Id, e.Nome))
                .ToListAsync(cancellationtoken);
                return estudantes;

            });

            rotasEstudantes.MapPut("{id}", async (Guid id, UpdateEstudanteRequest request, AppDbContext context, CancellationToken cancellationtoken) =>
            {
                var estudante = await context
                .Estudantes
                .SingleOrDefaultAsync(e => e.Id == id, cancellationtoken);

                if (estudante == null)
                    return Results.NotFound();

                estudante.AtualizarNome(request.Nome);

                await context.SaveChangesAsync(); //Não é necessário chamar o Update o próprio EF entende que houve alteração no objeto as no Track, seria para desabilitar o rastreio

                var estudanteDTO = new EstudanteDTO(estudante.Id, estudante.Nome);
                return Results.Ok(estudante);

            });

            rotasEstudantes.MapDelete("{id}", async (Guid id, AppDbContext context, CancellationToken cancellationtoken) => {
            var estudante = await context
                .Estudantes
                .SingleOrDefaultAsync(e => e.Id == id, cancellationtoken);

                if (estudante == null)
                    return Results.NotFound();

                estudante.Desativar();
                await context.SaveChangesAsync();

                return Results.Ok();
                
            });

        }
    }
}
