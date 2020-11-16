
// Form With Steps

$.fn.create_form = function () {
    const sections = this.find(".form-carousel-custom > section").toArray();
    const steps = () => this.find(".form-steps > li").toArray();
    const ctrl = {
        _index: 0,
        onChange() {
            //sections.forEach(el => $(el).css("left", `-${100 * this._index}%`));
            steps(this._index).forEach((el, idx) => {
                if (idx < this._index) {
                    $(el).addClass("complete");
                } else {
                    $(el).removeClass("complete");
                }
            });
        },
        next() {
            this._index = Math.min(this._index + 1, sections.length - 1);
            this.onChange();
        },
        prev() {
            this._index = Math.max(this._index - 1, 0);
            this.onChange();
        }
    };

    this.find(".form-steps").append(
        sections.map(el => `<li id="${$(el).data("step")}" >${$(el).data("step")}</li>`).join("")
    );

    this.find(".next-btn").on("click", () => ctrl.next());
    this.find(".prev-btn").on("click", () => ctrl.prev());
};

$(".register-form").create_form();

$("#FoodSearch").submit(function (e) {
    e.preventDefault();
});

$('#Search').click(function () {
    max = 10;
    searchFood()
});

$(document).ready(function () {
    loadExisting();
    $('.list-group-item').tooltip();
    $('.bg-transparent').tooltip();
});



function checkString(str,i) {
    if (/^[0-9,.]*$/.test(str) && str.length > 0) {
        $("#HAmount").val(str*i);
        return true;
    }
    return false;
}

function searchFood() {
    let Searchterm = $("#SearchTerm").val();
    let foodType = $("#foodType").val();
    let apiSource = "";
    let userId = $("#inputUserId").val();
    let verifiedLogo = '<svg xmlns="http://www.w3.org/2000/svg" width="24" fill="green" height="24" viewBox="0 0 24 24"><path d="M23.334 11.96c-.713-.726-.872-1.829-.393-2.727.342-.64.366-1.401.064-2.062-.301-.66-.893-1.142-1.601-1.302-.991-.225-1.722-1.067-1.803-2.081-.059-.723-.451-1.378-1.062-1.77-.609-.393-1.367-.478-2.05-.229-.956.347-2.026.032-2.642-.776-.44-.576-1.124-.915-1.85-.915-.725 0-1.409.339-1.849.915-.613.809-1.683 1.124-2.639.777-.682-.248-1.44-.163-2.05.229-.61.392-1.003 1.047-1.061 1.77-.082 1.014-.812 1.857-1.803 2.081-.708.16-1.3.642-1.601 1.302s-.277 1.422.065 2.061c.479.897.32 2.001-.392 2.727-.509.517-.747 1.242-.644 1.96s.536 1.347 1.17 1.7c.888.495 1.352 1.51 1.144 2.505-.147.71.044 1.448.519 1.996.476.549 1.18.844 1.902.798 1.016-.063 1.953.54 2.317 1.489.259.678.82 1.195 1.517 1.399.695.204 1.447.072 2.031-.357.819-.603 1.936-.603 2.754 0 .584.43 1.336.562 2.031.357.697-.204 1.258-.722 1.518-1.399.363-.949 1.301-1.553 2.316-1.489.724.046 1.427-.249 1.902-.798.475-.548.667-1.286.519-1.996-.207-.995.256-2.01 1.145-2.505.633-.354 1.065-.982 1.169-1.7s-.135-1.443-.643-1.96zm-12.584 5.43l-4.5-4.364 1.857-1.857 2.643 2.506 5.643-5.784 1.857 1.857-7.5 7.642z"/></svg>';
    let BearerToken = sessionStorage.getItem("authToken");

    switch (foodType) {
        case "nfood":
            $.get("../api/foods/search", { term: Searchterm, max: max }).done(function (results) {
                let main = $("#FoodResults")
                main.text("");
                if (results.length > 0) {
                    $("#ResultsFound").addClass("d-none");
                    let heading = $("<h6>Results:</h6>")
                    let ul = $('<ul class="list-group"></ul>')
                    main.append(heading);
                    main.append(ul);
                    $.each(results, function (index, value) {
                        let li = $('<li class="list-group-item d-flex list-group-item-action justify-content-between align-items-center btn-outline-dark ripple" data-placement="left" title="Add item to intake">' + value.name + '</li>')
                        li.on("click", function () {
                            setFood(value.id, value.name, foodType),
                                getFoodDetails(value)
                        });
                        let btn = $('<button type="button" class= "btn btn-light bg-transparent border-0 hidden-xs-down not-visible") data-placement="right" title="Add item to bookmarks"><svg xmlns="http://www.w3.org/2000/svg" class="svgBookmark" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-delete"><path d="M21 4H8l-7 8 7 8h13a2 2 0 0 0 2-2V6a2 2 0 0 0-2-2z"></path></svg></button >')

                        $.get("api/FoodBookmarks/" + value.id, { userId: userId }).done(function (checkBookmark) {
                            if (checkBookmark.length > 0) {
                                $(li).find('.svgBookmark').css("fill", "gold");
                                $(li).find('.svgBookmark').css("stroke", "gold");
                                $(li).find('svg').removeClass('svgBookmark');
                            }
                        });

                        $(btn).hover(function () {
                            $(this).find('.svgBookmark').css("fill", "gold");
                            $(this).find('.svgBookmark').css("stroke", "gold");
                        }, function () {
                            $(this).find('.svgBookmark').css("fill", "none");
                            $(this).find('.svgBookmark').css("stroke", "currentColor");
                        })

                        btn.on("click", function () {



                            setBookmark(value.id)

                            //e.stopPropagation();
                            var productCategory = new Object();
                            productCategory.UserId = userId;
                            if (foodType == "nfood") {
                                productCategory.FoodId = value.id;
                            } else if (foodType == "cfood") {
                                productCategory.CustomFoodId = value.id;
                            }

                            $.postJSON = function (url, data, callback) {
                                return jQuery.ajax({
                                    headers: {
                                        'Accept': 'application/json',
                                        'Content-Type': 'application/json'
                                    },
                                    'type': 'POST',
                                    'url': url,
                                    'data': JSON.stringify(data),
                                    'dataType': 'json',
                                    'success': callback
                                });
                            };


                            if (confirm('Are you sure you want to add this food to bookmarks?')) {
                                $.postJSON("../api/FoodBookmarks", productCategory, function (postResult) {
                                    if (postResult == null) {
                                        alert("Food is already in your bookmarks");
                                    } else {
                                        alert(value.name + " has been added to your bookmarks");
                                    }

                                })
                            }

                            getFoodDetails()
                            //$("#bookmarkForm").submit();
                        });

                        li.append(btn);
                        ul.append(li);
                    })
                } else {
                    $("#ResultsFound").removeClass("d-none");
                    $('#ResultsFound').click(function () {
                        $('#foodType').val("efood");
                        searchFood();
                        $('#foodCheckBox').prop('checked', true);
                    })
                }
            })
            break;
        case "cfood":
            $.get("../api/CustomFoods/search", { term: Searchterm, max: max }).done(function (results) {
                let main = $("#FoodResults")
                main.text("");
                if (results.length > 0) {
                    $("#ResultsFound").addClass("d-none");
                    let heading = $("<h6>Results:</h6>")
                    let ul = $('<ul class="list-group"></ul>')
                    main.append(heading);
                    main.append(ul);
                    $.each(results, function (index, value) {
                        let li = $('<li class="list-group-item d-flex list-group-item-action justify-content-between align-items-center btn-outline-dark ripple" data-placement="left" title="Add item to intake" >' + value.name + '</li>')
                        li.on("click", function () {
                            setFood(value.id, value.name, foodType),
                                getFoodDetails(value)
                        });

                        let btn = $('<button type="button" class= "btn btn-light bg-transparent border-0 hidden-xs-down not-visible")><svg xmlns="http://www.w3.org/2000/svg" class="svgBookmark" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-delete"><path d="M21 4H8l-7 8 7 8h13a2 2 0 0 0 2-2V6a2 2 0 0 0-2-2z"></path></svg></button >')

                        $.get("api/FoodBookmarks/" + value.id, { userId: userId }).done(function (checkBookmark) {
                            if (checkBookmark.length > 0) {
                                $(li).find('.svgBookmark').css("fill", "gold");
                                $(li).find('.svgBookmark').css("stroke", "gold");
                                $(li).find('svg').removeClass('svgBookmark');
                            }
                        });

                        $(btn).hover(function () {
                            $(this).find('.svgBookmark').css("fill", "gold");
                            $(this).find('.svgBookmark').css("stroke", "gold");
                        }, function () {
                            $(this).find('.svgBookmark').css("fill", "none");
                            $(this).find('.svgBookmark').css("stroke", "currentColor");
                        })

                        btn.on("click", function () {



                            setBookmark(value.id)

                            //e.stopPropagation();
                            var productCategory = new Object();
                            productCategory.UserId = userId;
                            if (foodType == "nfood") {
                                productCategory.FoodId = value.id;
                            }
                            else if (foodType == "cfood") {
                                productCategory.CustomFoodId = value.id;
                            }

                            $.postJSON = function (url, data, callback) {
                                return jQuery.ajax({
                                    headers: {
                                        'Accept': 'application/json',
                                        'Content-Type': 'application/json'
                                    },
                                    'type': 'POST',
                                    'url': url,
                                    'data': JSON.stringify(data),
                                    'dataType': 'json',
                                    'success': callback
                                });
                            };


                            if (confirm('Are you sure you want to add this food to bookmarks?')) {
                                $.postJSON("../api/FoodBookmarks", productCategory, function (postResult) {
                                    if (postResult == null) {
                                        alert("Food is already in your bookmarks");
                                    } else {
                                        alert(value.name + " has been added to your bookmarks");
                                    }

                                })
                            }

                            getFoodDetails(value);
                            //$("#bookmarkForm").submit();
                        });

                        li.append(btn);
                        ul.append(li);
                    })
                } else {
                    $("#ResultsFound").removeClass("d-none");
                    $('#ResultsFound').click(function () {
                        $('#foodType').val("efood");
                        searchFood();
                        $('#foodCheckBox').prop('checked', true);
                    })
                }
            })
            break;
        case "bfood":
            $.get("../api/FoodBookmarks", { userId: userId }).done(function (bResults) {
                let main = $("#FoodResults")
                main.text("");
                if (bResults.length > 0) {
                    $("#ResultsFound").addClass("d-none");
                    let heading = $("<h6>Results:</h6>")
                    let ul = $('<ul class="list-group"></ul>')
                    main.append(heading);
                    main.append(ul);

                    $.each(bResults, function (index, value) {
                        let foodname = "";
                        if (value.food != null) {
                            foodname = value.food.name
                        }
                        else if (value.customFood != null) {
                            foodname = value.customFood.name
                        }
                        let foodTypeCheck = $('#foodCheckBox')
                        let li = $('<li class="list-group-item d-flex list-group-item-action justify-content-between align-items-center btn-outline-dark ripple" data-placement="left" title="Add item to intake" >' + foodname + '</li>')
                        li.on("click", function () {

                            if (value.food != null) {
                                foodTypeCheck.prop("checked", false);
                                setFood(value.foodId, value.food.name, "nfood");
                                
                            } else if (value.customFood != null) {
                                foodTypeCheck.prop("checked", true);
                                setFood(value.customFoodId, value.customFood.name, "efood");
                                
                            }

                        });

                        let btn = $('<button type="button" class= "btn btn-light bg-transparent border-0 hidden-xs-down not-visible")><svg xmlns="http://www.w3.org/2000/svg" class="svgBookmark" width="24" height="24" viewBox="0 0 24 24" fill="gold" stroke="gold" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-delete"><path d="M21 4H8l-7 8 7 8h13a2 2 0 0 0 2-2V6a2 2 0 0 0-2-2z"></path></svg></button >')
                        $(btn).hover(function () {
                            $(this).find('.svgBookmark').css("fill", "none");
                            $(this).find('.svgBookmark').css("stroke", "currentColor");
                        }, function () {
                            $(this).find('.svgBookmark').css("fill", "gold");
                            $(this).find('.svgBookmark').css("stroke", "gold");
                        })

                        btn.on("click", function (e) {
                            //remove bookmark
                            //setBookmark(value.id)
                            if (confirm('Are you sure you want to remove this food from bookmarks?')) {
                                e.stopPropagation();
                                $.ajax({
                                    url: 'api/FoodBookmarks/' + value.id,
                                    type: 'DELETE',
                                    success: function (data) {
                                        alert(foodname + " has been removed from bookmarks");
                                        searchFood();
                                    }
                                });

                            }


                        });
                        li.append(btn);
                        ul.append(li);
                    })
                } else {
                    $("#ResultsFound").removeClass("d-none");
                }
            });
            break;
        case "efood":
            $.get("../api/CustomFoods/searchExternal", { term: Searchterm, bearerToken :  BearerToken}).done(function (externalResults) {
                var jsonResults = JSON.parse(externalResults);
                let main = $("#FoodResults")
                main.text("");
                console.log(jsonResults);
                if (jsonResults.foods.food.length > 0) {
                    $("#ResultsFound").addClass("d-none");
                    let heading = $("<h6>Results:</h6>")
                    let ul = $('<ul class="list-group"></ul>')
                    main.append(heading);
                    main.append(ul);
                    $.each(jsonResults.foods.food, function (index, value) {
                        let li = $('<li class="list-group-item d-flex list-group-item-action justify-content-between align-items-center btn-outline-dark ripple" data-placement="left" title="Add item to intake" >' + value.food_name + '</li>')
                        li.on("click", function () {
                            //setFood(value.food_id, value.food_name, foodType)
                            $.get("../api/CustomFoods/getExternalFood", { exFoodId: value.food_id, BearerToken}).done(function (Result) {
                                let jsonFood = JSON.parse(Result);
                                console.log(jsonFood);
                                $.postJSON = function (url, data, callback) {
                                    return jQuery.ajax({
                                        headers: {
                                            'Accept': 'application/json',
                                            'Content-Type': 'application/json'
                                        },
                                        'type': 'POST',
                                        'url': url,
                                        'data': JSON.stringify(data),
                                        'dataType': 'json',
                                        'success': callback
                                    });
                                };

                                var storeExternalFood = new Object();
                                storeExternalFood.Id = jsonFood.food.food_id;
                                storeExternalFood.Name = jsonFood.food.food_name;

                                if (jsonFood.food.servings.serving.length == undefined || jsonFood.food.servings.serving.length == null || jsonFood.food.servings.serving.length == 0) {
                                    storeExternalFood.Amount = parseFloat(jsonFood.food.servings.serving.metric_serving_amount) || 1;
                                    storeExternalFood.Enegry = (parseFloat(jsonFood.food.servings.serving.calories) * 4.184) || 0;
                                    storeExternalFood.Protein = parseFloat(jsonFood.food.servings.serving.protein) || 0;
                                    storeExternalFood.Fat = parseFloat(jsonFood.food.servings.serving.fat) || 0;
                                    storeExternalFood.Carbohydrates = parseFloat(jsonFood.food.servings.serving.carbohydrate) || 0;
                                    storeExternalFood.DietaryFibre = parseFloat(jsonFood.food.servings.serving.fiber) || 0;
                                    storeExternalFood.Sugars = parseFloat(jsonFood.food.servings.serving.sugar) || 0;
                                    storeExternalFood.SFA = parseFloat(jsonFood.food.servings.serving.saturated_fat) || 0;
                                    storeExternalFood.MUFA = parseFloat(jsonFood.food.servings.serving.monounsaturated_fat) || 0;
                                    storeExternalFood.PUFA = parseFloat(jsonFood.food.servings.serving.polyunsaturated_fat) || 0;
                                    storeExternalFood.Calcium = parseFloat(jsonFood.food.servings.serving.calcium) || 0;
                                    storeExternalFood.Cholesterol = parseFloat(jsonFood.food.servings.serving.cholesterol) || 0;
                                    storeExternalFood.Iron = parseFloat(jsonFood.food.servings.serving.iron) || 0;
                                    storeExternalFood.Potassium = parseFloat(jsonFood.food.servings.serving.potassium) || 0;
                                    storeExternalFood.Sodium = parseFloat(jsonFood.food.servings.serving.sodium) || 0;
                                    storeExternalFood.VitaminA = parseFloat(jsonFood.food.servings.serving.vitamin_a) || 0;
                                    storeExternalFood.VitaminC = parseFloat(jsonFood.food.servings.serving.vitamin_c) || 0;
                                }
                                else {
                                    storeExternalFood.Amount = parseFloat(jsonFood.food.servings.serving[0].metric_serving_amount) || 1;
                                    storeExternalFood.Enegry = (parseFloat(jsonFood.food.servings.serving[0].calories) * 4.184) || 0;
                                    storeExternalFood.Protein = parseFloat(jsonFood.food.servings.serving[0].protein) || 0;
                                    storeExternalFood.Fat = parseFloat(jsonFood.food.servings.serving[0].fat) || 0;
                                    storeExternalFood.Carbohydrates = parseFloat(jsonFood.food.servings.serving[0].carbohydrate) || 0;
                                    storeExternalFood.DietaryFibre = parseFloat(jsonFood.food.servings.serving[0].fiber) || 0;
                                    storeExternalFood.Sugars = parseFloat(jsonFood.food.servings.serving[0].sugar) || 0;
                                    storeExternalFood.SFA = parseFloat(jsonFood.food.servings.serving[0].saturated_fat) || 0;
                                    storeExternalFood.MUFA = parseFloat(jsonFood.food.servings.serving[0].monounsaturated_fat) || 0;
                                    storeExternalFood.PUFA = parseFloat(jsonFood.food.servings.serving[0].polyunsaturated_fat) || 0;
                                    storeExternalFood.Calcium = parseFloat(jsonFood.food.servings.serving[0].calcium) || 0;
                                    storeExternalFood.Cholesterol = parseFloat(jsonFood.food.servings.serving[0].cholesterol) || 0;
                                    storeExternalFood.Iron = parseFloat(jsonFood.food.servings.serving[0].iron) || 0;
                                    storeExternalFood.Potassium = parseFloat(jsonFood.food.servings.serving[0].potassium) || 0;
                                    storeExternalFood.Sodium = parseFloat(jsonFood.food.servings.serving[0].sodium) || 0;
                                    storeExternalFood.VitaminA = parseFloat(jsonFood.food.servings.serving[0].vitamin_a) || 0;
                                    storeExternalFood.VitaminC = parseFloat(jsonFood.food.servings.serving[0].vitamin_c) || 0;
                                }
                                
                                $.postJSON("../api/CustomFoods", storeExternalFood, function (postResult) {
                                    console.log(postResult);
                                });

                                getFoodDetails(storeExternalFood);

                                $("#VariationResults").addClass("pb-2");
                                $("#CustomFoodId").val(jsonFood.food.food_id);
                                $("#FoodName").text(jsonFood.food.food_name);
                                $("#Food").addClass("complete");
                                $("#FoodInput").removeClass("d-none");
                                $("#FoodResults").text("");
                                $("#FoodSearch").addClass("d-none");

                                let main = $("#VariationResults")
                                main.text("");
                                let heading = $("<h6>Serving size:</h6>")
                                main.append(heading);

                                let ul = $('<ul class="list-group pb-2"></ul>')
                                main.append(ul);
                                //display default serving sizes if they exist for this food
                                if (jsonFood.food.servings.serving.length == undefined || jsonFood.food.servings.serving.length == null || jsonFood.food.servings.serving.length == 0) {

                                    var storeExternalVariation = new Object();
                                    storeExternalVariation.Id = jsonFood.food.servings.serving.serving_id;
                                    storeExternalVariation.Name = jsonFood.food.servings.serving.serving_description;
                                    storeExternalVariation.Amount = parseFloat(jsonFood.food.servings.serving.metric_serving_amount) || 1;
                                    storeExternalVariation.CustomFoodId = jsonFood.food.food_id;

                                    $.postJSON("../api/CustomFoods/variations", storeExternalVariation, function (postResult) {
                                        console.log(postResult);
                                    });


                                    let name = jsonFood.food.servings.serving.serving_description;
                                    
                                    let serving = " - " + jsonFood.food.servings.serving.metric_serving_amount; + " " + jsonFood.food.servings.serving.metric_serving_unit || "";
                                    if (jsonFood.food.servings.serving.metric_serving_amount == undefined || jsonFood.food.servings.serving.metric_serving_amount == null || jsonFood.food.servings.serving.metric_serving_amount == 0) {
                                        serving = "";
                                    }
                                    

                                    let description = name + serving;
                                    let li = $('<li id="' + jsonFood.food.servings.serving.serving_id + '" class="variations list-group-item d-flex list-group-item-action justify-content-between align-items-center btn-outline-dark ripple">' + description + '</li>')
                                    li.on("click", function () {
                                        setVariation(jsonFood.food.servings.serving.serving_id, jsonFood.food.servings.serving.serving_description, jsonFood.food.servings.serving.metric_serving_amount || 1)
                                    });
                                    let btn = $('<button type="button" class="btn btn-light bg-transparent border-0 hidden-xs-down not-visible")></button >')
                                    btn.on("click", function () {
                                        //setVariation(value.id, value.name, value.amount)
                                    });
                                    li.append(btn);
                                    ul.append(li);
                                }
                                else {

                                    $.each(jsonFood.food.servings.serving, function (index, value) {

                                        var storeExternalVariation = new Object();
                                        storeExternalVariation.Id = value.serving_id;
                                        storeExternalVariation.Name = value.serving_description;
                                        storeExternalVariation.Amount = parseFloat(value.metric_serving_amount);
                                        storeExternalVariation.CustomFoodId = jsonFood.food.food_id;

                                        $.postJSON("../api/CustomFoods/variations", storeExternalVariation, function (postResult) {
                                            console.log(postResult);
                                        });

                                        let description = value.serving_description + " - " + value.metric_serving_amount + " " + value.metric_serving_unit;
                                        let li = $('<li id="' + value.serving_id + '" class="variations list-group-item d-flex list-group-item-action justify-content-between align-items-center btn-outline-dark ripple">' + description + '</li>')
                                        li.on("click", function () {
                                            setVariation(value.serving_id, value.serving_description, value.metric_serving_amount)
                                        });
                                        let btn = $('<button type="button" class="btn btn-light bg-transparent border-0 hidden-xs-down not-visible")></button >')
                                        btn.on("click", function () {
                                            //setVariation(value.id, value.name, value.amount)
                                        });
                                        li.append(btn);
                                        ul.append(li);
                                    })
                                }
                                    
                                
                                    

                                //manual input for serving size
                                if ($("#NewIntake").val() == "False") {
                                    main.append('<input autocomplete="off" id="Amount" type="number" step="any" class="form-control text-center" value="' + $("#HMeasure").val() + '" placeholder="Serving Size in grams..." />')
                                } else {
                                    main.append('<input autocomplete="off" id="Amount" type="number" step="any" class="form-control text-center" placeholder="Serving Size in grams..." />')
                                }
                                $(".ripple").init_ripple();

                                $("#Amount").on("input", function () {
                                    getAmount()
                                });
                            });
                        });

                        let btn = $('<button type="button" class= "btn btn-light bg-transparent border-0 hidden-xs-down not-visible")><svg xmlns="http://www.w3.org/2000/svg" class="svgBookmark" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-delete"><path d="M21 4H8l-7 8 7 8h13a2 2 0 0 0 2-2V6a2 2 0 0 0-2-2z"></path></svg></button >')

                        $.get("api/FoodBookmarks/" + value.food_id, { userId: userId }).done(function (checkBookmark) {
                            if (checkBookmark.length > 0) {
                                $(li).find('.svgBookmark').css("fill", "gold");
                                $(li).find('.svgBookmark').css("stroke", "gold");
                                $(li).find('svg').removeClass('svgBookmark');
                            }
                        });

                        $(btn).hover(function () {
                            $(this).find('.svgBookmark').css("fill", "gold");
                            $(this).find('.svgBookmark').css("stroke", "gold");
                        }, function () {
                            $(this).find('.svgBookmark').css("fill", "none");
                            $(this).find('.svgBookmark').css("stroke", "currentColor");
                        })

                        btn.on("click", function () {

                            $.get("../api/CustomFoods/getExternalFood", { exFoodId: value.food_id, BearerToken }).done(function (Result) {
                                let jsonFood = JSON.parse(Result);
                                console.log(jsonFood);
                                $.postJSON = function (url, data, callback) {
                                    return jQuery.ajax({
                                        headers: {
                                            'Accept': 'application/json',
                                            'Content-Type': 'application/json'
                                        },
                                        'type': 'POST',
                                        'url': url,
                                        'data': JSON.stringify(data),
                                        'dataType': 'json',
                                        'success': callback
                                    });
                                };

                                var storeExternalFood = new Object();
                                storeExternalFood.Id = jsonFood.food.food_id;
                                storeExternalFood.Name = jsonFood.food.food_name;

                                if (jsonFood.food.servings.serving.length == undefined || jsonFood.food.servings.serving.length == null || jsonFood.food.servings.serving.length == 0) {
                                    storeExternalFood.Amount = parseFloat(jsonFood.food.servings.serving.metric_serving_amount) || 1;
                                    storeExternalFood.Enegry = (parseFloat(jsonFood.food.servings.serving.calories) * 4.184) || 0;
                                    storeExternalFood.Protein = parseFloat(jsonFood.food.servings.serving.protein) || 0;
                                    storeExternalFood.Fat = parseFloat(jsonFood.food.servings.serving.fat) || 0;
                                    storeExternalFood.Carbohydrates = parseFloat(jsonFood.food.servings.serving.carbohydrate) || 0;
                                    storeExternalFood.DietaryFibre = parseFloat(jsonFood.food.servings.serving.fiber) || 0;
                                    storeExternalFood.Sugars = parseFloat(jsonFood.food.servings.serving.sugar) || 0;
                                    storeExternalFood.SFA = parseFloat(jsonFood.food.servings.serving.saturated_fat) || 0;
                                    storeExternalFood.MUFA = parseFloat(jsonFood.food.servings.serving.monounsaturated_fat) || 0;
                                    storeExternalFood.PUFA = parseFloat(jsonFood.food.servings.serving.polyunsaturated_fat) || 0;
                                    storeExternalFood.Calcium = parseFloat(jsonFood.food.servings.serving.calcium) || 0;
                                    storeExternalFood.Cholesterol = parseFloat(jsonFood.food.servings.serving.cholesterol) || 0;
                                    storeExternalFood.Iron = parseFloat(jsonFood.food.servings.serving.iron) || 0;
                                    storeExternalFood.Potassium = parseFloat(jsonFood.food.servings.serving.potassium) || 0;
                                    storeExternalFood.Sodium = parseFloat(jsonFood.food.servings.serving.sodium) || 0;
                                    storeExternalFood.VitaminA = parseFloat(jsonFood.food.servings.serving.vitamin_a) || 0;
                                    storeExternalFood.VitaminC = parseFloat(jsonFood.food.servings.serving.vitamin_c) || 0;
                                }
                                else {
                                    storeExternalFood.Amount = parseFloat(jsonFood.food.servings.serving[0].metric_serving_amount) || 1;
                                    storeExternalFood.Enegry = (parseFloat(jsonFood.food.servings.serving[0].calories) * 4.184) || 0;
                                    storeExternalFood.Protein = parseFloat(jsonFood.food.servings.serving[0].protein) || 0;
                                    storeExternalFood.Fat = parseFloat(jsonFood.food.servings.serving[0].fat) || 0;
                                    storeExternalFood.Carbohydrates = parseFloat(jsonFood.food.servings.serving[0].carbohydrate) || 0;
                                    storeExternalFood.DietaryFibre = parseFloat(jsonFood.food.servings.serving[0].fiber) || 0;
                                    storeExternalFood.Sugars = parseFloat(jsonFood.food.servings.serving[0].sugar) || 0;
                                    storeExternalFood.SFA = parseFloat(jsonFood.food.servings.serving[0].saturated_fat) || 0;
                                    storeExternalFood.MUFA = parseFloat(jsonFood.food.servings.serving[0].monounsaturated_fat) || 0;
                                    storeExternalFood.PUFA = parseFloat(jsonFood.food.servings.serving[0].polyunsaturated_fat) || 0;
                                    storeExternalFood.Calcium = parseFloat(jsonFood.food.servings.serving[0].calcium) || 0;
                                    storeExternalFood.Cholesterol = parseFloat(jsonFood.food.servings.serving[0].cholesterol) || 0;
                                    storeExternalFood.Iron = parseFloat(jsonFood.food.servings.serving[0].iron) || 0;
                                    storeExternalFood.Potassium = parseFloat(jsonFood.food.servings.serving[0].potassium) || 0;
                                    storeExternalFood.Sodium = parseFloat(jsonFood.food.servings.serving[0].sodium) || 0;
                                    storeExternalFood.VitaminA = parseFloat(jsonFood.food.servings.serving[0].vitamin_a) || 0;
                                    storeExternalFood.VitaminC = parseFloat(jsonFood.food.servings.serving[0].vitamin_c) || 0;
                                }

                                $.postJSON("../api/CustomFoods", storeExternalFood, function (postResult) {
                                    console.log(postResult);
                                });

                                getFoodDetails(storeExternalFood);

                                setBookmark(value.food_id)
                            })

                            

                            //e.stopPropagation();
                            var productCategory = new Object();
                            productCategory.UserId = userId;
                            if (foodType == "nfood") {
                                productCategory.FoodId = value.id;
                            } else if (foodType == "efood") {
                                productCategory.CustomFoodId = value.food_id;
                            }

                            $.postJSON = function (url, data, callback) {
                                return jQuery.ajax({
                                    headers: {
                                        'Accept': 'application/json',
                                        'Content-Type': 'application/json'
                                    },
                                    'type': 'POST',
                                    'url': url,
                                    'data': JSON.stringify(data),
                                    'dataType': 'json',
                                    'success': callback
                                });
                            };


                            if (confirm('Are you sure you want to add this food to bookmarks?')) {
                                $.postJSON("../api/FoodBookmarks", productCategory, function (postResult) {
                                    if (postResult == null) {
                                        alert("Food is already in your bookmarks");
                                    } else {
                                        alert(value.food_name + " has been added to your bookmarks");
                                    }

                                })
                            }

                            
                            //$("#bookmarkForm").submit();
                        });

                        li.append(btn);
                        ul.append(li);
                    })
                } else {
                    $("#ResultsFound").removeClass("d-none");
                }
            })
            break;
    }
}

function searchVariation(id, type) {
    let foodType = type;

    switch (foodType) {
        case "nfood":
            $.get("../api/foods/variation", { id: id }).done(function (results) {
                let main = $("#VariationResults")
                main.text("");
                let heading = $("<h6>Serving size:</h6>")
                main.append(heading);

                //display default serving sizes if they exist for this food
                if (results.length > 0) {
                    let ul = $('<ul class="list-group pb-2"></ul>')
                    main.append(ul);

                    $.each(results, function (index, value) {
                        let description = value.name + " - " + value.amount + " g";
                        let li = $('<li id="' + value.id + '" class="variations list-group-item d-flex list-group-item-action justify-content-between align-items-center btn-outline-dark ripple">' + description + '</li>')
                        li.on("click", function () {
                            setVariation(value.id, value.name, value.amount)
                        });
                        let btn = $('<button type="button" class="btn btn-light bg-transparent border-0 hidden-xs-down not-visible")></button >')
                        btn.on("click", function () {
                            setVariation(value.id, value.name, value.amount)
                        });
                        li.append(btn);
                        ul.append(li);
                    })
                }

                //manual input for serving size
                if ($("#NewIntake").val() == "False") {
                    main.append('<input autocomplete="off" id="Amount" type="number" step="any" class="form-control text-center" value="' + $("#HMeasure").val() + '" placeholder="Serving Size in grams..." />')
                } else {
                    main.append('<input autocomplete="off" id="Amount" type="number" step="any" class="form-control text-center" placeholder="Serving Size in grams..." />')
                }
                $(".ripple").init_ripple();

                $("#Amount").on("input", function () {
                    getAmount()
                });
            })
            break;
        case "efood":
            $.get("../api/CustomFoods/variation", { id: id }).done(function (results) {
                console.log(results);
                let main = $("#VariationResults")
                main.text("");
                let heading = $("<h6>Serving size:</h6>")
                main.append(heading);

                //display default serving sizes if they exist for this food
                if (results.length > 0) {
                    let ul = $('<ul class="list-group pb-2"></ul>')
                    main.append(ul);

                    $.each(results, function (index, value) {
                        let description = value.name + " - " + value.amount + " g";
                        let li = $('<li id="' + value.id + '" class="variations list-group-item d-flex list-group-item-action justify-content-between align-items-center btn-outline-dark ripple">' + description + '</li>')
                        li.on("click", function () {
                            setVariation(value.id, value.name, value.amount)
                        });
                        let btn = $('<button type="button" class="btn btn-light bg-transparent border-0 hidden-xs-down not-visible")></button >')
                        btn.on("click", function () {
                            setVariation(value.id, value.name, value.amount)
                        });
                        li.append(btn);
                        ul.append(li);
                    })
                }

                //manual input for serving size
                if ($("#NewIntake").val() == "False") {
                    main.append('<input autocomplete="off" id="Amount" type="number" step="any" class="form-control text-center" value="' + $("#HMeasure").val() + '" placeholder="Serving Size in grams..." />')
                } else {
                    main.append('<input autocomplete="off" id="Amount" type="number" step="any" class="form-control text-center" placeholder="Serving Size in grams..." />')
                }
                $(".ripple").init_ripple();

                $("#Amount").on("input", function () {
                    getAmount()
                });
            })
            break;
    }
}

function unselectVariations() {
    $('.variations').each(function () {
        $(this).addClass("btn-outline-dark");
        $(this).removeClass("bg-success");
    });
    $("#VariationId").val("");
    $("#HVariationName").val("");
}

function getAmount() {
    unselectVariations();
    if ($("#Amount").val() == "" || $("#Amount").val() <= 0) {
        $("#Serving").removeClass("complete");
        $("#QuantityInput").addClass("d-none");
        $("#intake-modal-finalsection").addClass("d-none");
        $("#viewDetailsBtn").addClass("disabled");
        $("#finalSubmit").addClass("disabled");
    } else {
        $("#intake-modal-finalsection").removeClass("d-none");
        $("#viewDetailsBtn").removeClass("disabled");
        $("#HAmount").val("")
        $("#HMeasure").val($("#Amount").val())

        let str = $("#Amount")
            .val()
            .toLowerCase()
            .replace(/ /g, "");
        $("#AmountConvert").text("");

        if (/^[0-9,.]*$/.test(str) && str.length > 0) {
            $("#HAmount").val(str);
        }

        if (
            !checkString(str.replace("g", ""), 1) &&
            !checkString(str.replace("kg", ""), 1000) &&
            !checkString(str.replace("l", ""), 1000) &&
            !checkString(str.replace("ml", ""), 1)
        ) {
            $("#Serving").removeClass("complete");
            $("#QuantityInput").addClass("d-none");
            //$("#AmountConvert").text("Please enter the correct units.");
        } else {
            $("#Serving").addClass("complete");
            $("#QuantityInput").removeClass("d-none");
        }
    }
}

function setFood(id, name, type) {
    if (id == "0") {
        max += 5
        searchFood()
        return;
    }
    let fId = "";
    //let foodType = $("#foodType").val();

    switch (type) {
        case "nfood":
            fId = $("#FoodId");
            break;
        case "efood":
            fId = $("#CustomFoodId");
            break;
    }


    $("#VariationResults").addClass("pb-2");
    fId.val(id);
    $("#FoodName").text(name);// Mcdonald's fails....
    $("#Food").addClass("complete");
    $("#FoodInput").removeClass("d-none");
    $("#FoodResults").text("");
    $("#FoodSearch").addClass("d-none");
    $("#viewDetailsBtn").removeClass("disabled");
    searchVariation(id, type)
}

function setBookmark(id) {
    let fId = "";
    let foodType = $("#foodType").val();
    if (foodType == "nfood") {
        fId = $("#nFoodBookmark");
        $("#cFoodBookmark").val = "";
    } else {
        fId = $("#cFoodBookmark");
        $("#nFoodBookmark").val = "";
    }

    fId.val(id);
}

function resetFood() {
    let fId = "";
    let foodType = $("#foodType").val();
    if (foodType == "nfood") {
        fId = $("#FoodId");
    } else {
        fId = $("#CustomFoodId");
    }
    $("#Food").removeClass("complete");
    $("#Serving").removeClass("complete");
    $("#Quantity").removeClass("complete");
    $("#VariationResults").text("");
    $("#VariationResults").removeClass("pb-2");
    fId.val("");
    $("#FoodName").text("");
    $("#FoodInput").addClass("d-none");
    $("#FoodSearch").removeClass("d-none");
    $("#SearchTerm").val("");
    $("#VariationId").val("");
    $("#HVariationName").val("");
    $("#Amount").val("");
    $("#Qty").val("");
    $("#HAmount").val("");
    $("#HMeasure").val("");
    $("#HQuantity").val("");
    $("#QuantityInput").addClass("d-none");
    $("#viewDetailsBtn").addClass("disabled");
    $("#intake-modal-finalsection").addClass("d-none");
}

function setVariation(id, name, amount) {
    unselectVariations();
    
    $("#" + id).removeClass("btn-outline-dark");
    $("#" + id).addClass("bg-success");
    $("#VariationId").val(id);
    $("#HVariationName").val(name);
    $("#Amount").val(amount);
    $("#HMeasure").val(amount);
    $("#HAmount").val(amount);
    $("#Serving").addClass("complete");
    $("#QuantityInput").removeClass("d-none");
    $("#intake-modal-finalsection").removeClass("d-none");
}

function ResetVariation() {
    $("#intake-modal-finalsection").addClass("d-none");
    $("#Serving").removeClass("complete");
    $("#VariationId").val("");
    $("#HVariationName").val("");
    $("#Amount").val("");
    $("#HQuantity").val("");
    $("#HAmount").val("");
    $("#HMeasure").val("");
    $("#Qty").val("");
}

function addQty(i) {
    $("#Qty").val($("#Qty").val() + i);
    validateQty();
}

$("#Qty").on("input", function () {
    validateQty();
});

$("#SearchTerm").on("input", function () {
    searchFood();
});

$("#MealType").on("change", function () {
    $("#HMealType").val($('select[name="MealType"]').val());
});

function updateMealType() {
    $("#HMealType").val($('select[name="MealType"]').val());
}

function submitForm() {
    //check whether form has been fully filled out
    if ($("#Qty").val().includes(".")) {
        $("#errorModalBody").text("Number of servings must be a whole number");
        $("#errorModal").modal("show");
    }
    else if ($("#Qty").val() <= 0 || $("#Amount").val() <= 0) {
        $("#errorModalBody").text("Serving size and number of servings must be larger than 0");
        $("#errorModal").modal("show");
    } else {
        $("#IntakeForm").submit();
    }
}

function validateQty() {
    $("#HQuantity").val($("#Qty").val())
    if ($("#Qty").val() <= 0) {
        $("#Quantity").removeClass("complete");
        $("#finalSubmit").addClass("disabled");
        $("#finalSubmit").removeClass("btn-success");

    } else {
        $("#Quantity").addClass("complete");
        $("#finalSubmit").addClass("btn-success");
        $("#finalSubmit").removeClass("disabled");
    }
}

function loadExisting() {
    if ($("#NewIntake").val() == "False") {
        let fId = "";
        let foodType = $("#foodType").val();
        if (foodType == "nfood" || foodType == null) {
            fId = $("#FoodId");
            foodType = "nfood";
        } else {
            fId = $("#CustomFoodId");
        }
        setFood(fId.val(), $("#FoodName").text(), foodType)
        
        $("#Qty").val($("#HQuantity").val())

        $("#Food").addClass("complete");
        $("#Serving").addClass("complete");
        $("#Quantity").addClass("complete");
        $("#intake-modal-finalsection").removeClass("d-none");
        $("#QuantityInput").removeClass("d-none");
        validateQty()
    }

    if (sessionStorage.getItem("authToken") == null) {
        $.get("../api/CustomFoods/token").done(function (token) {
            var jsonToken = JSON.parse(token);
            var authtoken = jsonToken.access_token;
            sessionStorage.setItem("authToken", authtoken);
        })
    }
}


$(".ripple")
    .toArray()
    .forEach(el => {
        $(el).append(`<span class="ink"></span>`);
        $(el).click(function (e) {
            let parent = $(this).parent();
            let ink = parent.find(".ink");

            //in case of quick double clicks
            //stop the previous animation
            ink.removeClass("animate");

            //set size of .ink
            if (!ink.height() && !ink.width()) {
                //use parent's width or height whichever
                //is larger for the diameter to make a circle
                //which can cover the entire element
                let size = Math.max(parent.outerWidth(), parent.outerHeight());
                ink.css({
                    height: size,
                    width: size
                });
            }

            //set the position and add class .animate
            ink
                .css({
                    top: e.pageY - parent.offset().top - ink.height() / 2 + "px",
                    left: e.pageX - parent.offset().left - ink.width() / 2 + "px"
                })
                .addClass("animate");
        });
    });

$.fn.init_ripple = function () {
    for (let el of this.toArray()) {
        let $el = $(el);

        $el.append(`<span class="ink"></span>`);
        $el.click(function (e) {
            let parent = $(this).parent();
            let ink = parent.find(".ink");

            //in case of quick double clicks
            //stop the previous animation
            ink.removeClass("animate");

            //set size of .ink
            if (!ink.height() && !ink.width()) {
                //use parent's width or height whichever
                //is larger for the diameter to make a circle
                //which can cover the entire element
                let size = Math.max(parent.outerWidth(), parent.outerHeight());
                ink.css({
                    height: size,
                    width: size
                });
            }

            //set the position and add class .animate
            ink
                .css({
                    top: e.pageY - parent.offset().top - ink.height() / 2 + "px",
                    left: e.pageX - parent.offset().left - ink.width() / 2 + "px"
                })
                .addClass("animate");
        });
    }
};

$(".ripple").init_ripple();

function getFoodDetails(value) {
    $("#ID").text(value.id);
    $("#Name").text(value.name);
    $("#Protein").text(value.protein == -1 ? "<0.1" : value.protein);
    $("#Carbohydrates").text(value.carbohydrates == -1 ? "<0.1" : value.carbohydrates);
    $("#Fat").text(value.fat == -1 ? "<0.1" : value.fat);
    $("#SlphalinolenicAcid").text(value.alphaLinolenicAcid == -1 ? "<0.1" : value.alphaLinolenicAcid);
    $("#BetaCarotene").text(value.betaCarotene == -1 ? "<0.1" : value.betaCarotene);
    $("#Calcium").text(value.calcium == -1 ? "<0.1" : value.calcium);
    $("#Cholesterol").text(value.cholesterol == -1 ? "<0.1" : value.cholesterol);
    $("#DietaryFibre").text(value.dietaryFibre == -1 ? "<0.1" : value.dietaryFibre);
    $("#DietaryFolate").text(value.dietaryFolate == -1 ? "<0.1" : value.dietaryFolate);
    $("#EnegryNIP").text(value.enegryNIP == -1 ? "<0.1" : value.enegryNIP);
    $("#Iodine").text(value.idodine == -1 ? "<0.1" : value.idodine);
    $("#Iron").text(value.iron == -1 ? "<0.1" : value.iron);
    $("#LionleicAcid").text(value.linoleicAcid == -1 ? "<0.1" : value.linoleicAcid);
    $("#MUFA").text(value.MUFA == -1 ? "<0.1" : value.MUFA);
    $("#Niacin").text(value.niacin == -1 ? "<0.1" : value.niacin);
    $("#Phosphorus").text(value.phosphorus == -1 ? "<0.1" : value.phosphorus);
    $("#Potassium").text(value.potassium == -1 ? "<0.1" : value.potassium);
    $("#PUFA").text(value.pUFA == -1 ? "<0.1" : value.pUFA);
    $("#Riboflavin").text(value.riboflavin == -1 ? "<0.1" : value.riboflavin);
    $("#Selenium").text(value.selenium == -1 ? "<0.1" : value.selenium);
    $("#SFA").text(value.sFA == -1 ? "<0.1" : value.sFA);
    $("#Sodium").text(value.sodium == -1 ? "<0.1" : value.sodium);
    $("#Starch").text(value.starch == -1 ? "<0.1" : value.starch);
    $("#Sugars").text(value.sugars == -1 ? "<0.1" : value.sugars);
    $("#Thiamin").text(value.thiamin == -1 ? "<0.1" : value.thiamin);
    $("#VitaminA").text(value.vitaminA == -1 ? "<0.1" : value.vitaminA);
    $("#VitaminB12").text(value.vitaminB12 == -1 ? "<0.1" : value.vitaminB12);
    $("#VitaminC").text(value.vitaminC == -1 ? "<0.1" : value.vitaminC);
    $("#VitaminD").text(value.vitaminD == -1 ? "<0.1" : value.vitaminD);
    $("#VitaminE").text(value.vitaminE == -1 ? "<0.1" : value.vitaminE);
    $("#Water").text(value.water == -1 ? "<0.1" : value.water);
    $("#Zinc").text(value.zinc == -1 ? "<0.1" : value.zinc);
}
