$.fn.create_form = function () {
    const sections = this.find(".form-carousel > section").toArray();
    const steps = () => this.find(".form-steps > li").toArray();
    const ctrl = {
        _index: 0,
        onChange() {
            $("#Amount").blur(); 
            $("#Qty").blur(); 
            sections.forEach(el => $(el).css("left", `-${100 * this._index}%`));
            steps(this._index).forEach((el, idx) => {
                if (idx < this._index) {
                    $(el).addClass("complete");

                } else {
                    $(el).removeClass("complete");
                }
            });
        },
        next() {
            if (this._index == 3) { 
                $("#IntakeForm").submit();
            }
            this._index = Math.min(this._index + 1, sections.length - 1);
            this.onChange();
            this.validateIntakeSection();
        },
        prev() {
            this._index = Math.max(this._index - 1, 0);
            this.onChange();
            this.validateIntakeSection();
        },
        validateIntakeSection() {
            switch (this._index) {
                case 0:
                    $("#VariationResults").addClass("d-none");
                    if ($("#FoodId").val() == "" && $("#FoodName").text() == "") {
                        $("#next").prop("disabled", true);
                    } else {
                        $("#next").prop("disabled", false);
                    }
                    break;
                case 1:
                    if ($("#VariationId").val() == "" && $("#Amount").val() == "") {
                        $("#next").prop("disabled", true);
                        $("#VariationResults").removeClass("d-none");
                    } else {
                        $("#next").prop("disabled", false);
                    }
                    break;
                case 2:
                    $("#CheckList").addClass("d-none");
                    $("#Qty").focus();
                    if ($("#Qty").val() == "") {
                        $("#next").prop("disabled", true);
                    } else {
                        $("#next").prop("disabled", false);
                    }
                    break;
                case 3:
                    $("#CheckList").removeClass("d-none");
                    $("#liFoodName").text($("#FoodName").text());
                    $("#liVariationName").text($("#VariationName").text());
                    $("#liAmount").text($("#HAmount").val());
                    $("#liQuantity").text($("#HQuantity").val());
                    //Process Post
                    break;
            }
            
        }
        
    };

    this.find(".form-steps").append(
        sections.map(el => `<li>${$(el).data("step")}</li>`).join("")
    );

    this.find(".next-btn").on("click", () => ctrl.next());
    this.find(".prev-btn").on("click", () => ctrl.prev());
};

$(".intake-form").create_form();

function SearchFood() {
    let Searchterm = $("#SearchTerm").val();
    $.get("../api/foods/search", { term: Searchterm, max: max }).done(function (results) {
        if (results.length > 0) {
            let data = ["foods"];
            data["foods"] = results;
            $("#ResultsFound").addClass("d-none");
            $("#FoodResults").removeClass("d-none");
            w3.displayObject("FoodResults", data);
        } else {
            $("#ResultsFound").removeClass("d-none");
            $("#FoodResults").addClass("d-none");
        }
    })
}

function SearchVariation(id) {
    $.get("../api/foods/variation", { id: id }).done(function (results) {
        results.unshift({ "id": "0", "name": "Custom Serving Size", "amount": "" });
        let data = ["variations"];
        data["variations"] = results;

        w3.displayObject("VariationResults", data);
    })
}

function setFood(id, name) {
    if (id == "0") {
        max += 10
        SearchFood()
        return;
    }
    $("#FoodId").val(id);
    $("#FoodName").text(name);
    $("#FoodInput").removeClass("d-none");
    $("#FoodResults").addClass("d-none");
    $("#FoodSearch").addClass("d-none");
    $("#next").prop("disabled", false);
    SearchVariation(id)
}

function ResetFood() {
    $("#FoodId").val("");
    $("#FoodName").text("");
    $("#FoodInput").addClass("d-none");
    $("#FoodSearch").removeClass("d-none");
    $("#SearchTerm").val("");
    $("#next").prop("disabled", true);
    $("#VariationId").val("");
    $("#Amount").val("");
    $("#Qty").val("");
    $("#HAmount").val("");
    $("#HQuantity").val("");
}

function setVariation(id, name, amount) {
    if (id == '0') {
        $("#InputAmount").removeClass("d-none");
        $("#Amount").val("");
        $("#HAmount").val("");
        $("#Amount").focus();
        $("#next").prop("disabled", true);
    } else {
        $("#InputAmount").addClass("d-none");
        $("#Amount").val(amount);
        $("#HAmount").val(amount);
        $("#next").prop("disabled", false);
    }
    $("#VariationInput").removeClass("d-none");
    $("#VariationResults").addClass("d-none");
    $("#VariationId").val(id);
    $("#VariationName").text(name);

}

function ResetVariation() {
    $("#VariationId").val("");
    $("#VariationName").text("");
    $("#VariationInput").addClass("d-none");
    $("#VariationResults").removeClass("d-none");
    $("#next").prop("disabled", true);
    $("#InputAmount").addClass("d-none");
    $("#Amount").val("");
    $("#Quantity").val("");
    $("#HAmount").val("");
    $("#Qty").val("");
}

$("#FoodSearch").submit(function (e) {
    e.preventDefault();
});

$('#Search').click(function () {
    SearchFood()
});

$(document).ready(function () {
    $("#Amount").on("input", function () {
        if ($("#Amount").val() == "") {
            $("#next").prop("disabled", true);
        } else {
            $("#HAmount").val($("#Amount").val())
            $("#next").prop("disabled", false);
        }
    });

    $("#Qty").on("input", function () {
        if ($("#Qty").val() == "") {
            $("#next").prop("disabled", true);
        } else {
            $("#HQuantity").val($("#Qty").val())
            $("#next").prop("disabled", false);
        }
    });
});