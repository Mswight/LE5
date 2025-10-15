using Microsoft.AspNetCore.Mvc;
using BlogAPI.Models;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;

        public PostController(ILogger<PostController> logger)
        {
            _logger = logger;
        }

        // Static field to store posts (in-memory database)
        private static List<Post> _posts = new List<Post>()
{
    new Post
    {
        Id = 1,
        Title = "Why I Love Debugging",
        Body = "Some people dread debugging, but I find it oddly satisfying. Tracing bugs feels like solving a mystery — every stack trace is a clue, every console log a breadcrumb. Today I squashed a nasty routing issue in Angular, and it felt like winning a mini battle.",
        DateCreated = DateTime.Now.AddDays(-6),
        UserName = "fatima_tech",
        FirstName = "Fatima",
        LastName = "Al-Mutairi"
    },
    new Post
    {
        Id = 2,
        Title = "Designing for Accessibility",
        Body = "I’ve been diving into accessible UI design lately. From color contrast to keyboard navigation, every detail matters. I redesigned a login form to support screen readers and RTL layouts — and it finally feels inclusive. Beauty and clarity should never exclude anyone.",
        DateCreated = DateTime.Now.AddDays(-4),
        UserName = "noura_ui",
        FirstName = "Noura",
        LastName = "Hassan"
    },
    new Post
    {
        Id = 3,
        Title = "Digital Forensics Lab: First Impressions",
        Body = "Today was my first hands-on session in the digital forensics lab. We verified tool hashes, documented chain of custody, and examined a mock evidence drive. It’s fascinating how technical precision meets legal responsibility — and I’m hooked.",
        DateCreated = DateTime.Now.AddDays(-2),
        UserName = "forensic_nerd",
        FirstName = "Omar",
        LastName = "Salem"
    },
    new Post
    {
        Id = 4,
        Title = "My Favorite VS Code Extensions",
        Body = "I can’t live without Prettier, GitLens, and Angular Snippets. They turn my chaotic code into something readable and elegant. I even created a bilingual cheat sheet for my team — one side in English, one in Arabic. It’s been a hit!",
        DateCreated = DateTime.Now.AddDays(-1),
        UserName = "code_crafter",
        FirstName = "Layla",
        LastName = "Rahman"
    },
    new Post
    {
        Id = 5,
        Title = "From Burnout to Breakthrough",
        Body = "Last week I hit a wall. Nothing worked, and I felt stuck. So I took a break, sketched out my app UI on paper, and came back with fresh eyes. Suddenly, the routing bug made sense. Sometimes clarity comes from stepping away.",
        DateCreated = DateTime.Now,
        UserName = "dev_diaries",
        FirstName = "Yousef",
        LastName = "Al-Kandari"
    }
};


        // ✅ NEW: GET api/post/list
        [HttpGet("list")]
        public IActionResult GetAllPosts()
        {
            _logger.LogInformation("Getting all posts via /list");
            return Ok(_posts.OrderByDescending(p => p.DateCreated));
        }

        // GET: api/post
        [HttpGet]
        public IEnumerable<Post> Get()
        {
            _logger.LogInformation("Getting all posts");
            return _posts.OrderByDescending(p => p.DateCreated);
        }

        // GET: api/post/1
        [HttpGet("{id}")]
        public ActionResult<Post> GetPost(int id)
        {
            _logger.LogInformation($"Getting post with id: {id}");
            var post = _posts.FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                _logger.LogWarning($"Post with id {id} not found");
                return NotFound();
            }

            return post;
        }

        // POST: api/post
        [HttpPost]
        public IActionResult AddPost([FromBody] Post newPost)
        {
            _logger.LogInformation("Adding new post");
            newPost.Id = _posts.Any() ? _posts.Max(p => p.Id) + 1 : 1;
            newPost.DateCreated = DateTime.Now;
            _posts.Add(newPost);

            return CreatedAtAction(nameof(GetPost), new { id = newPost.Id }, newPost);
        }

        // PUT: api/post/1
        [HttpPut("{id}")]
        public IActionResult UpdatePost(int id, [FromBody] Post post)
        {
            _logger.LogInformation($"Updating post with id: {id}");
            var existingPost = _posts.FirstOrDefault(p => p.Id == id);

            if (existingPost == null)
            {
                _logger.LogWarning($"Post with id {id} not found for update");
                return NotFound();
            }

            existingPost.Title = post.Title;
            existingPost.Body = post.Body;
            existingPost.UserName = post.UserName;
            existingPost.FirstName = post.FirstName;
            existingPost.LastName = post.LastName;

            return NoContent();
        }

        // DELETE: api/post/1
        [HttpDelete("{id}")]
        public IActionResult DeletePost(int id)
        {
            _logger.LogInformation($"Deleting post with id: {id}");
            var postToRemove = _posts.FirstOrDefault(p => p.Id == id);

            if (postToRemove == null)
            {
                _logger.LogWarning($"Post with id {id} not found for deletion");
                return NotFound();
            }

            _posts.Remove(postToRemove);
            return NoContent();
        }
    }
}
