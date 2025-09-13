using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Blog.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TagsController : Controller
    {
        private readonly ITagRepository tagRepository;

        public TagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        [HttpGet]
        public IActionResult Add() => View();

        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            ValidateAddTagRequest(addTagRequest);

            if (!ModelState.IsValid)
                return View();

            await tagRepository.AddAsync(new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            });

            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public async Task<IActionResult> List(string? searchQuery)
        {
            ViewBag.SearchQuery = searchQuery;
            var tags = await tagRepository.GetAllAsync(searchQuery);
            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await tagRepository.GetAsync(id);

            if (tag is not null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };

                return View(editTagRequest);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var updatedTag = await tagRepository.UpdateAsync(new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            });

            if (updatedTag is not null)
            {
                return RedirectToAction(nameof(List));
            }
            else
            {

            }

            return RedirectToAction(nameof(Edit), new { id = editTagRequest.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var deletedTag = await tagRepository.DeleteAsync(editTagRequest.Id);

            if (deletedTag is not null)
            {
                return RedirectToAction(nameof(List));
            }

            return RedirectToAction(nameof(Edit), new { id = editTagRequest.Id });
        }

        #region Private Methods

        private void ValidateAddTagRequest(AddTagRequest addTagRequest)
        {
            if (addTagRequest.Name is not null && addTagRequest.DisplayName is not null)
            {
                if (addTagRequest.Name == addTagRequest.DisplayName)
                    ModelState.AddModelError("DisplayName", "Name cannot be the same as Display Name.");
            }
        }

        #endregion
    }
}
