namespace Amsberg_digital_universe.Features.Posts.CreatePost;

public record CreatePostRequest(string Content,Guid IdempotencyKey, DateTime? PublishTime);