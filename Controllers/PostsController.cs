using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PostMe.Data;
using PostMe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PostMe.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PostsController(ApplicationDbContext db)
        {
            _db = db;
        }

        
        public async Task<IActionResult> Index()
        {
            ClaimsPrincipal currentUser = User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var posts = from p in _db.Posts orderby p.Date descending where p.UserID == currentUserID select p;

            return View(await posts.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Post post)
        {
            ClaimsPrincipal currentUser = User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var currentUserName = currentUser.FindFirst(ClaimTypes.Name).Value;
            post.UserID = currentUserID;
            post.UserName = currentUserName;
            post.Date = DateTime.Now;

            _db.Posts.Add(post);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Update(int? id)
        {
            ClaimsPrincipal currentUser = User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var post = _db.Posts.Find(id);

            if (post == null)
            {
                return NotFound();
            }

            if(post.UserID != currentUserID)
            {
                return NotFound();
            }
            else
            {
                return View(post);
            } 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePost(Post post)
        {
            if (post == null)
            {
                return NotFound();
            }

            ClaimsPrincipal currentUser = User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var currentUserName = currentUser.FindFirst(ClaimTypes.Name).Value;
            if(post.UserID != currentUserID)
            {
                return NotFound();
            }
            else
            {
                post.UserID = currentUserID;
                post.UserName = currentUserName;
                post.Date = DateTime.Today;

                _db.Posts.Update(post);
                _db.SaveChanges();
                return RedirectToAction("Index");
            } 
        }

        public IActionResult Delete(int? id)
        {
            ClaimsPrincipal currentUser = User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var post = _db.Posts.Find(id);

            if(post == null)
            {
                return NotFound();
            }

            if (post.UserID != currentUserID)
            {
                return NotFound();
            }
            else
            {
                return View(post);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            ClaimsPrincipal currentUser = User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var post = _db.Posts.Find(id);

            if(post.UserID != currentUserID)
            {
                return NotFound();
            }
            else
            {
                _db.Posts.Remove(post);
                _db.SaveChanges();
                return RedirectToAction("Index");
            } 
        }
    }
}
