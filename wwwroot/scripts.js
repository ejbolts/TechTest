document.addEventListener("DOMContentLoaded", function () {
    document.querySelector(".convert-button").addEventListener("click", convertNumber);
    document.querySelector(".reset-button").addEventListener("click", resetFields);
});

function convertNumber() {
    const number = document.getElementById("numberInput").value;
    const spinner = document.getElementById("spinner");
    spinner.style.visibility = "visible";
    document.getElementById("output").innerText = "";
    const maxNumber = 9223372036854775807n; // long.MaxValue
    const minNumber = -9223372036854775808n; // long.MinValue

    // Client-side validation
    if (!number) {
        document.getElementById("output").innerText =
            "Error: Please enter a number.";
        spinner.style.visibility = "hidden";
        return;
    }

    if (isNaN(number)) {
        document.getElementById("output").innerText =
            "Error: Invalid number format.";
        spinner.style.visibility = "hidden";
        return;
    }

    if (number.includes(".") && number.split(".")[1].length > 2) {
        document.getElementById("output").innerText =
            "Error: Please enter a number with at most 2 decimal places.";
        spinner.style.visibility = "hidden";
        return;
    }

    const numericValue = parseFloat(number);

    if (numericValue > maxNumber || numericValue < minNumber) {
        document.getElementById("output").innerText =
            "Error: Number is out of range.";
        spinner.style.visibility = "hidden";
        return;
    }

    fetch(`/api/NumberToWords/convert?number=${number}`)
        .then((response) => {
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            return response.json();
        })
        .then((data) => {
            if (data.words) {
                document.getElementById("output").innerText = data.words;
            } else {
                document.getElementById("output").innerText =
                    "Error: " + (data.error || "Unknown error");
            }
        })
        .catch((error) => {
            document.getElementById("output").innerText =
                "Error: " + error.message;
        })
        .finally(() => {
            spinner.style.visibility = "hidden";
        });
}

function resetFields() {
    document.getElementById("numberInput").value = "";
    document.getElementById("output").innerText = "";
}
