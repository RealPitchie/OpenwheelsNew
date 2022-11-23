using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Wheels.Domain.Models;
using Wheels.Persistence;

namespace Wheels.API.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks; 
using HtmlAgilityPack;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed; 
using Newtonsoft.Json;
 
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [EnableCors("MyPolicy")]
    public class PostsController : ControllerBase
    {
        private  PostsContext _context; 
        readonly DbSet<Post> _posts;
        readonly DbSet<Comment> _comments;
        public PostsController(PostsContext context)
        {
            _context = context; 
            _posts = _context.Set<Post>();
            _comments = _context.Set<Comment>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1)
        {
            var posts = await _posts.OrderByDescending(p => p.Posted).ToListAsync();
            int pageSize = 15;
            var items = posts.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(items);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _posts.OrderByDescending(p => p.Posted).ToListAsync();  
            return Ok(posts);
        }
        [HttpGet]
        public async Task<IActionResult> GetByRating(int page = 1)
        {
            var posts = await _posts.OrderByDescending(p => p.Rating).ToListAsync();
            const int pageSize = 15;
            var items = posts.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(items);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var post = await _posts.FirstAsync(p => p.Id == id);  
            return Ok(post);
        }
        // POST api/posts
        [HttpPost]
        public async Task<ActionResult<Post>> Post(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return Ok(post);
        }
        [HttpGet]
        public async Task<IActionResult> GetComments(string postId)
        {
            var comments = await _comments.Where(c => c.PostId == postId).ToListAsync();

            return Ok(comments);
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
        }
        // [HttpGet("redis")]
        // public async Task<IActionResult> Get(int page = 1)
        // {
        //     var cacheKey = "PostList";
        //     string serializedPostList;
        //     var PostList = new List<Post>();
        //     var redisPostList = await distributedCache.GetAsync(cacheKey); 
        //     if (redisPostList != null)
        //     {
        //         serializedPostList = Encoding.UTF8.GetString(redisPostList);
        //         PostList = JsonConvert.DeserializeObject<List<Post>>(serializedPostList);
        //     }
        //     else
        //     {
        //         PostList = await _context.Posts.OrderByDescending(p => p.Posted).ToListAsync();
        //         serializedPostList = JsonConvert.SerializeObject(PostList);
        //         redisPostList = Encoding.UTF8.GetBytes(serializedPostList);
        //         var options = new DistributedCacheEntryOptions()
        //             .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
        //             .SetSlidingExpiration(TimeSpan.FromMinutes(2));
        //         await distributedCache.SetAsync(cacheKey, redisPostList, options);
        //     }
        //     int pageSize = 15;
        //     var items = PostList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //     return Ok(items);
        // }
        [HttpGet]
        public async Task<IActionResult> Parse()
        {
            for(var i = 160000; i <= 163861; i++)
            {
                var post =  await ParsePost(i);
                if (post.Id == "emptyPost") continue;
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
                //Thread.Sleep(5000);

            }

            return Ok();
        }
        // POST api/ParsePost
        [HttpGet]
        public async Task<Post> ParsePost(int postNumber)
        {
            try{
                var client = new HttpClient();
                var response = await client.GetAsync("https://www.f1news.ru/news/f1-" + postNumber + ".html");
                var pageContents = await response.Content.ReadAsStringAsync();
                var pageDocument = new HtmlDocument();
                pageDocument.LoadHtml(pageContents);
                var headlineText = pageDocument.DocumentNode
                    .SelectSingleNode("(//h1[contains(@class,'post_title')])[1]").InnerText;
                var postImage = pageDocument.DocumentNode
                    .SelectSingleNode("(//img[contains(@itemprop,'contentUrl url')])[1]")
                    .Attributes.First(a => a.Name == "src").Value;
                var postDate = pageDocument.DocumentNode
                    .SelectSingleNode("(//div[contains(@class,'post_date')])[1]").
                    Attributes.First(a => a.Name == "content").Value;
                var posted = DateTime.Parse(postDate);
                var postText = pageDocument.DocumentNode
                    .SelectNodes("(//div[contains(@class,'post_content')]/child::p)");
                Post newPost = new Post{
                    Id = Guid.NewGuid().ToString(),
                    Posted = posted,
                    Title = headlineText,
                    ImageFile = new []{ postImage },
                    Text = postText.Select(item => item.InnerText).ToArray(),
                    Rating = 0
                };
                return newPost;
            }
            catch
            {
                // ignored
            }

            return new Post{Id = "emptyPost"};
        }

        // [HttpGet]
        // public async Task<IActionResult> GetMongoDatabases()
        // {
        //     var posts = await _context.Posts.ToListAsync();
        //     var client = new MongoClient("mongodb://localhost:27017");
        //     var db = client.GetDatabase("Wheels");
        //     var data = db.GetCollection<Post>("Posts");
        //     if (posts != null) await data.InsertManyAsync(posts);
        //     return Ok(data);
        // }

        [HttpGet]
        public async Task<IActionResult> TimeComparisonPSQL()
        {
            var sw = new Stopwatch();
            var times = new List<string>();
            var posts = new List<Post>();
            sw.Start();
            posts = await _context.Posts.ToListAsync();
            times.Add("Postgresql time: " + sw.ElapsedMilliseconds);
            sw.Stop();
            return Ok(times);
        }
        [HttpGet]
        public async Task<IActionResult> TimeComparisonMongo()
        {
            var sw = new Stopwatch();
            var times = new List<string>();
            var posts = new List<Post>();
            sw.Start();
            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("Wheels");
            var data = db.GetCollection<Post>("Posts");
            using var cursor = await data.FindAsync(new BsonDocument());
            posts = await cursor.ToListAsync();
            times.Add("Mongo time: " + sw.ElapsedMilliseconds);
            sw.Stop();
            return Ok(times);
        }
    } 