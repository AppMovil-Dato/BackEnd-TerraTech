using Microsoft.Extensions.Logging;
using NovaTech.TerraTech.Platform.CommunityManagement.Application.Errors;
using NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.Queries;
using NovaTech.TerraTech.Platform.CommunityManagement.Domain.Repositories;
using NovaTech.TerraTech.Platform.Shared.Application.Model;
using NovaTech.TerraTech.Platform.Shared.Domain.Repositories;

namespace NovaTech.TerraTech.Platform.CommunityManagement.Application.Services;
public class CommentService(ICommentRepository commentRepository, IUnitOfWork unitOfWork, ILogger<CommentService> logger) : ICommentService
{
    public async Task<Result<Comment>> Handle(CreateCommentCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var comment = new Comment(command);
            await commentRepository.AddAsync(comment, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Comment>.Success(comment);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating comment for ProfileId {TargetProfileId}", command.TargetProfileId);
            return Result<Comment>.Failure(CommunityError.InvalidComment, "The provided comment data is invalid");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating comment for ProfileId {TargetProfileId}", command.TargetProfileId);
            return Result<Comment>.Failure(CommunityError.DatabaseError, "An unexpected error occurred while creating the comment");
        }
    }

    public async Task<IEnumerable<Comment>> Handle(GetCommentsByTargetProfileIdQuery query, CancellationToken cancellationToken = default)
    {
        return await commentRepository.FindByTargetProfileIdAsync(query.TargetProfileId, cancellationToken);
    }

    public async Task<Comment?> Handle(GetCommentByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await commentRepository.FindByIdAsync(query.Id, cancellationToken);
    }

    public async Task<Result<Comment>> Handle(UpdateCommentCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var comment = await commentRepository.FindByIdAsync(command.Id, cancellationToken);
            if (comment == null)
                return Result<Comment>.Failure(CommunityError.NotFound, "The comment was not found");
            comment.UpdateContent(command.Content, command.Rating);
            commentRepository.Update(comment);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Comment>.Success(comment);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating comment {CommentId}", command.Id);
            return Result<Comment>.Failure(CommunityError.DatabaseError, "An unexpected error occurred while updating the comment");
        }
    }

    public async Task<bool> Handle(DeleteCommentCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var comment = await commentRepository.FindByIdAsync(command.Id, cancellationToken);
            if (comment == null)
                return false;
            commentRepository.Remove(comment);
            await unitOfWork.CompleteAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting comment {Id}", command.Id);
            return false;
        }
    }
}
