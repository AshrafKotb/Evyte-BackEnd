// aos animation
$(function () {
  AOS.init();
});



// navbar
window.onscroll = function () {
  if (window.scrollY >= 50) {
    $("nav").addClass("bgMainColor opacity-75").removeClass("bg-dark bg-opacity-25");

  } else {
    $("nav").removeClass("bgMainColor opacity-75").addClass("bg-dark bg-opacity-25");
  }
};  

$(".nav-item.dropdown").on("mouseenter", function () {
  $(".dropdown-menu").removeClass("show");
  $(this).find(".dropdown-menu").addClass("show");

  $(this).on("mouseleave", function () {
    $(".dropdown-menu").removeClass("show");
  });
});

// countDown
(function () {
  const second = 1000,
        minute = second * 60,
        hour = minute * 60,
        day = hour * 24;

  //I'm adding this section so I don't have to keep updating this pen every year :-)
  //remove this if you don't need it
  let today = new Date(),
      dd = String(today.getDate()).padStart(2, "0"),
      mm = String(today.getMonth() + 1).padStart(2, "0"),
      yyyy = today.getFullYear(),
      nextYear = yyyy + 1,
      // if you wont change day month/day/
      dayMonth = "05/25/",
      birthday = dayMonth + yyyy;
  
  today = mm + "/" + dd + "/" + yyyy;
  if (today > birthday) {
    birthday = dayMonth + nextYear;
  }
  //end
  
  const countDown = new Date(birthday).getTime(),
      x = setInterval(function() {    

        const now = new Date().getTime(),
              distance = countDown - now;

        document.getElementById("days").innerText = Math.floor(distance / (day)),
          document.getElementById("hours").innerText = Math.floor((distance % (day)) / (hour)),
          document.getElementById("minutes").innerText = Math.floor((distance % (hour)) / (minute)),
          document.getElementById("seconds").innerText = Math.floor((distance % (minute)) / second);

        //do something later when date is reached
        if (distance < 0) {
          document.getElementById("countdown").style.display = "none";
          clearInterval(x);
        }
        //seconds
      }, 0)
  }());


// gallary
document.addEventListener("DOMContentLoaded", function () {
  let mixer = mixitup(".gallary");
});

// gallary box
(function(){
let imgs = Array.from(document.querySelectorAll(".gallary img"));

let gallaryBox = document.querySelector('.gallaryBox');
let currentIndex = 0;

$('.gallary img').on('click' , function(e){
  let currentImg = $(this).attr('src');
  currentIndex = imgs.indexOf(e.target);
  
  $('.gallaryBox').removeClass('d-none');
  $('body').addClass('overflow-hidden');
  $(this).attr('src');
  $('.gallaryBox img').attr('src' , currentImg )
});

$('.closeBtn').on('click' , close);
$('.nextBtn').on('click' , next);
$('.prevBtn').on('click' , prev);


$(document).on("click", function (e) {
  if (e.target ==  gallaryBox) {
     close();
    }
});

function close() {
  $('.gallaryBox').addClass('d-none');
  $('body').removeClass('overflow-hidden');
  
}

function next() {
    currentIndex++;

   if (currentIndex == imgs.length) {
      currentIndex = 0;
   }

   $('.gallaryBox img').attr('src' , imgs[currentIndex].src); 

}

function prev() {
    currentIndex--;

   if (currentIndex < 0) {
      currentIndex = imgs.length -1;
   }

   $('.gallaryBox img').attr('src' , imgs[currentIndex].src); 

}

// keyboard buttons
document.addEventListener("keydown", function (e) {
  if (e.key == "ArrowLeft") {
     prev();
  } else if (e.key == "ArrowRight") {
     next();
  } else if (e.key == "Escape") {
     close();
  }

});

})();


// switch ltr to rtl

$(document).ready(function() {
  $('.dropdown-menu .ar').on('click', function() {
      $('html').attr('dir', 'rtl');
      $('head').append(`<link rel="stylesheet" href="./css/rtl.css">`);
  });

  $('.dropdown-menu .en').on('click', function() {
      $('html').attr('dir', 'ltr');
      $('link[rel="stylesheet"][href="./css/rtl.css"]').remove();
  });
});


// wedding form
// choose or write Qoute

$(document).ready(function() {
  $('input[name="chooseOrWriteQoute"]').on('change', function() {     
    $(this).attr('id');

      $(`.row > .transition`).addClass('d-none');
      $(`.row > .${$(this).attr('id')}`).toggleClass('d-none');
  });

});