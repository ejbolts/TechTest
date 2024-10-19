function convertNumber() {
    const number = document.getElementById("numberInput").value;
    const maxNumber = 9223372036854775807n; // long.MaxValue
    const minNumber = -9223372036854775808n; // long.MinValue
    // Client-side validation
    if (!number) {
        document.getElementById("output").innerText =
            "Error: Please enter a number.";
        return;
    }
    if (number < 0) {
        document.getElementById("output").innerText =
            "Error: Please enter a positive number.";
        return;
    }

    if (isNaN(number)) {
        document.getElementById("output").innerText =
            "Error: Invalid number format.";
        return;
    }

    const numericValue = parseFloat(number);

    if (numericValue > maxNumber || numericValue < minNumber) {
        document.getElementById(
            "output"
        ).innerText = `Error: Number is out of range.`;
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
        });
}
function resetFields() {
    document.getElementById("numberInput").value = "";
    document.getElementById("output").innerText = "";
}