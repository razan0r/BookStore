﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModel
{
    public class CategoryVM
    {

        public int Id { get; set; }


        [Required(ErrorMessage ="plz enter name")]
        [MaxLength(30,ErrorMessage ="max 30")]
        [Remote("CheckName",null,ErrorMessage ="existsss")]

        public string Name { get; set; } = null!;

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}
