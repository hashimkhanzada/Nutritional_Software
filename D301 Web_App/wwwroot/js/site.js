
    // Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
    // for details on configuring this project to bundle and minify static web assets.

    // Write your Javascript code.

    // Form With Steps

    // const FORM_DEFAULTS = {};

//initialize tooltip
$(document).ready(function () {
    $('.tooltippy').tooltip();
    $('.btn').tooltip();
    $('.large-button-gradient').tooltip();
    $('li').tooltip();
    $('button').tooltip();
    $('#changeButton').tooltip();
    $('input').tooltip();
    $('select').tooltip();
    $('option').tooltip();

});

    $.fn.disable_inputs = function (val = true) {
        for (let el of this.find(":input, a").toArray()) {
            if (val) {
                $(el).attr("tabindex", "-1");
            } else {
                $(el).removeAttr("tabindex");
            }
        }
    };

    $.fn.init_forms = function (opts = {}) {
        // let {  } = $.extend(true, FORM_DEFAULTS, opts);

        for (let el of this.toArray()) {
            let $el = $(el);

            let sections = $el.find(".form-carousel > section").toArray();
            let steps = () => $el.find(".form-steps > li").toArray();
            let submitCustomFood = $el.find(".form-carousel");

            let index = 0;
            let update = idx => {
                // update the index
                index = idx;

                // set the section css
                // to move the sections to the correct location
                sections.forEach((section, idx) => {
                    let $section = $(section);

                    $section.css("left", `-${100 * index}%`);
                    $section.removeClass("current");
                    $section.disable_inputs(true);

                    if (idx == index) {
                        $section.addClass("current")
                        $section.disable_inputs(false);
                    }
                });

                // toggle the "complete" status tag
                // on all the required steps
                steps().forEach((el, idx) => {
                    $(el).removeClass("complete");
                    idx < index && $(el).addClass("complete");
                });

                // update the next btn
                $el.find(".next-btn").val(
                    index === sections.length - 1 ? "Finish" : "Continue"
                );
            };

            let next = () => {
                if (index === sections.length - 1) {
                    if (submitCustomFood.hasClass("shadow")) {
                        $("#AddCustomFoodForm").submit();
                    } else {
                        $("form").first().submit();
                    }

                    for (let [idx, el] of sections.entries()) {
                        if ($(el).children(".field-validation-error:empty").length) {
                            update(idx + 1);
                        }
                    }
                } else {
                    update(Math.min(index + 1, sections.length - 1));
                }
            };
            let prev = () => update(Math.max(index - 1, 0));

            let validate = callback => {
                return e => {
                    console.log("Validating Form Section");
                    e.preventDefault();

                    for (let input of $(sections[index]).find("input").toArray()) {
                        if (!$(input).valid()) {
                            console.error("Input Validation Failed... Halting Progression", input);
                            return false;
                        }
                        console.log("Input Validation Passed", input);
                    }

                    callback(e);
                };
            };

            $el
                .find(".form-steps")
                .append(sections.map(el => `<li>${$(el).data("step")}</li>`).join(""));

            $el
                .find(":input")
                .unbind()
                .keypress(e => {
                    if (e.which === 13) $el.find(".next-btn").click();
                });

            $el
                .find(".next-btn")
                .unbind()
                .on("click", validate(next));

            $el
                .find(".prev-btn")
                .unbind()
                .on("click", prev);

            update(0);
        }
    };

    // Goal Bar

    const GOAL_DEFAULTS = {
        multiplier: 1
    };

    $.fn.init_goal_bars = function (opts = {}) {
        let { multiplier } = $.extend(true, GOAL_DEFAULTS, opts);

        for (let el of this.toArray()) {
            let $el = $(el);

            // save original text
            if ($el.attr("data-original-text") === undefined) {
                $el.attr("data-original-text", $el.text());
            }

            let goal = parseFloat($el.attr("data-goal"));
            let value = Math.min(parseFloat($el.attr("data-value")), goal * multiplier);
            let msg =
                $el.attr("data-original-text") ||
                `${((value / goal) * 100).toFixed()}% of ${goal}`;


            $el.empty().append(`
      <div class="goal-bar-text">${msg}</div>
      <div 
        class="goal-bar-fill" 
        style="width: ${(value / (goal * multiplier)) * 100}%;"
      >
      </div>
    `);
        }
    };

    // Progress Bar

    const PROGRESS_DEFAULTS = {
        thresholds: {
            "bg-success": 65,
            "bg-warning": 69,
            "bg-danger": 90
        }
    };

    $.fn.init_progress_bars = function (opts = {}) {
        let { thresholds } = $.extend(true, PROGRESS_DEFAULTS, opts);

        for (let el of this.toArray()) {
            let $el = $(el);

            // save original text
            if ($el.attr("data-original-text") === undefined) {
                $el.attr("data-original-text", $el.text());
            }

            let value = parseFloat($el.attr("data-value"));
            let msg =
                $el.attr("data-original-text") || `${(value * 100).toFixed()}/100`;

            $el.empty().append(`
      <div class="progress-bar-text">${msg}</div>
      <div class="progress-bar-fill" style="width: ${value * 100}%;"></div>
    `);

            Object.keys(thresholds).forEach(k => $el.removeClass(k));
            Object.entries(thresholds).forEach(([key, threshold], idx) => {
                if (value * 100 < threshold && idx === 0) {
                    $el.addClass(key);
                }

                if (value * 100 < threshold) return;
                $el.addClass(key);
            });
        }
    };

    // Ripple Effect

    $.fn.init_ripple = function () {
        for (let el of this.toArray()) {
            let $el = $(el);

            $el.append(`<span class="ink"></span>`);
            $el.click(function (e) {
                let $parent = $(this);
                let $ink = $parent.find(".ink");

                //in case of quick double clicks
                //stop the previous animation
                $ink.removeClass("animate");

                //set size of .ink
                if (!$ink.height() && !$ink.width()) {
                    //use parent's width or height whichever
                    //is larger for the diameter to make a circle
                    //which can cover the entire element
                    let size = Math.max($parent.outerWidth(), $parent.outerHeight());
                    $ink.css({
                        height: size,
                        width: size
                    });
                }

                //set the position and add class .animate
                $ink
                    .css({
                        top: e.pageY - $parent.offset().top - $ink.height() / 2 + "px",
                        left: e.pageX - $parent.offset().left - $ink.width() / 2 + "px"
                    })
                    .addClass("animate");
            });
        }
    };

    $(".form-wrapper").init_forms();
    $(".goal-bar").init_goal_bars();
    $(".progress-bar").init_progress_bars();
    $(".ripple").init_ripple();

    //nav styling below this

    //makes nav stick to top of browser window

    //window.onscroll = function () { stickyNav() };

    //function stickyNav() {
    //    var sticky = navbar.offsetTop;

    //    if (window.pageYOffset >= sticky) {
    //        document.querySelector('header').classList.add("sticky")
    //    } else {
    //        document.querySelector('header').classList.remove("sticky");
    //    }
    //}

    //makes nav items change colour if you're on that page

    $(function () {
        $('.nav-link-active').each(function () {
            if (this.href.split("?")[0] == window.location.href.split("?")[0]) {
                $(this).addClass('active');
                $(this).parents('li').addClass('active');
                $(this).children('.nav-svg').addClass('active');
            }
        });
    });

    //three functions below change profile colouring based on hover over on child/sibling elements

    //$('#dropdowncontent').on('mouseover', function () {
    //    $(this).parent().addClass('is-profile-hover');
    //    $('#profileIcon').addClass('is-profile-hover');
    //    $('#profileIconTrainee').addClass('is-profileTrainee-hover');
    //}).on('mouseout', function () {
    //    $(this).parent().removeClass('is-profile-hover');
    //    $('#profileIcon').removeClass('is-profile-hover');
    //    $('#profileIconTrainee').removeClass('is-profileTrainee-hover');

    //})

    //$('#profileIconGroup').on('mouseover', function () {
    //    $('#profileBadge').addClass('is-profile-hover-badge');
    //}).on('mouseout', function () {
    //    $('#profileBadge').removeClass('is-profile-hover-badge');
    //})

    //$('#profileIconTraineeGroup').on('mouseover', function () {
    //    $('#profileBadgeTrainee').addClass('is-profile-hover-badge');
    //    $('#traineeSVG').addClass('svg-trainee-hover');
    //}).on('mouseout', function () {
    //    $('#profileBadgeTrainee').removeClass('is-profile-hover-badge');
    //    $('#traineeSVG').removeClass('svg-trainee-hover');
    //})

$(document).ready(function () {

    var el = $(".dropdown-content-intake");

    $(".intake-item-container").on("click", function (e) {

        var x = e.pageX - 15;
        var y = e.pageY - 15;



        el.css("left", x);
        el.css("top", y);

    })
})


function toggle_visibility(id) {

    var e = document.getElementById(id);
    console.log("click");
    if (e.style.display == 'flex')
        e.style.display = 'none';
    else
        e.style.display = 'flex';
    
    window.addEventListener('click', function (e) {
        if (document.getElementById(e).contains(e.target)) {
            console.log("not targeted item")
        } else {
            e.style.display = 'none';
        }
    });

    e.addEventListener("mouseleave", function (event) {
        e.style.display = 'none';
    })


}
//adds gradient on hover to food items


function mouseover(e) {
    

    var gradientItem = document.getElementById(e);
    

    gradientItem.onmousemove = (e) => {
        const x = e.pageX - e.target.offsetLeft
        const y = e.pageY - e.target.offsetTop

        $(gradientItem).css('--x', `${x}px`)
        $(gradientItem).css('--y', `${y}px`)
        
    }

    
}

var foodItemList = document.querySelector(".food-item-list")

if (foodItemList.offsetHeight < foodItemList.scrollHeight) {
    $('#foodItemListId').addClass('food-item-overflow');
} else {
    $('#foodItemListId').removeClass('food-item-overflow');
}

function navbarBrand() {

}