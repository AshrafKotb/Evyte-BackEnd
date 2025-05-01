// aos animation
$(function () {
  AOS.init({
    offset: 200, 
    delay: 0, 
    duration: 2000, 
    easing: 'ease-in-out',
    once: false
  });
});

// navbar
window.onscroll = function () {
  if (window.scrollY >= 50) {
    $("nav").addClass("bg-light").removeClass("bg-dark bg-opacity-25");
    $("#banner").addClass("d-none");
    $(".nav-link").removeClass("navColor");
    $(".navbar-brand.white").addClass("d-none");
    $(".navbar-brand.black").removeClass("d-none");

  } else {
    $("nav").removeClass("bg-light").addClass("bg-dark bg-opacity-25");
    $("#banner").removeClass("d-none");
    $(".nav-link").addClass("navColor");
    $(".navbar-brand.black").addClass("d-none");
    $(".navbar-brand.white").removeClass("d-none");
  }
};

$(".nav-item.dropdown").on("mouseenter", function () {
  $(".dropdown-menu").removeClass("show");
  $(this).find(".dropdown-menu").addClass("show");

  $(this).on("mouseleave", function () {
    $(".dropdown-menu").removeClass("show");
  });
});

// important people
document.addEventListener("DOMContentLoaded", function () {
  let mixer = mixitup(".importantPeople");
});

$(document).ready(function(){
  $(".frindSlider").slick({
    autoplay: true,
    dots: true,
    arrows: false,
    speed: 500,
    slidesToShow: 1,
    slickCurrentSlide: arguments,
    responsive: [
      
      {
        breakpoint: 768,
        settings: {
          arrows: false,
          slidesToShow: 1,
        },
      },
    ],
  });

  $(".giftSlider").slick({
    autoplay: true,
    arrows: false,
    dots: true,
    speed: 500,
    slidesToShow: 4,
    slickCurrentSlide: arguments,
    responsive: [
      {
        breakpoint: 992,
        settings: {
          arrows: false,
          slidesToShow: 3,
        },
      },
      {
        breakpoint: 768,
        settings: {
          arrows: false,
          slidesToShow: 2,
        },
      },
      {
        breakpoint: 480,
        settings: {
          arrows: false,
          centerMode: true,
          centerPadding: "50px",
          slidesToShow: 1,
        },
      },
    ],
  });
})

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

// gallery  
(function(){

  // gallary box slider
  
  let imgs = Array.from(document.querySelectorAll(".gallary img"));
  
  let gallaryBox = document.querySelector('.gallaryBox');
  let currentIndex = 0;
  
  $('.gallary .showImg').on('click' , function(e){
    let currentImg = $(this).siblings().attr('src');
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
  
  }());


