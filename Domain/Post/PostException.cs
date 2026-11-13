namespace Amsberg_digital_universe.Features.Posts.CreatePostDraft;

public class PostException:Exception
{
    public PostException() : base(){}
    public PostException(string message) : base(message){}
    public PostException(string message, Exception inner) : base(message, inner){}
}