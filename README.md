## Architectural Decisions

### Post creation   
__Problem__: While creating a post, there is a room for race condition.  
__Decision__: Added Idempotency Key to Post aggregate. User creates a post draft with unique key, we count every try to
create post with the same key as a success.