function GetFiltersHome()
{
    let CheckInDate = document.getElementById("CheckInDate").value;
    let CheckOutDate = document.getElementById("CheckOutDate").value;
    let NumberOfGuests = document.getElementById("guestNLabel").value;
    let Ordering = document.getElementById("Sorting").value;
    let MinimumPrice = document.getElementById("minPrice").value;
    let MaximumPrice = document.getElementById("maxPrice").value;
    let RoomC = document.getElementById("RoomCheckFilter").checked;
    let FSuiteC = document.getElementById("FSuiteCheckFilter").checked;
    let VSuiteC = document.getElementById("VSuiteCheckFilter").checked;
    let HtlAprtmntC = document.getElementById("HtlAprtmntCheckFilter").checked;
    let VilaC = document.getElementById("VilaCheckFilter").checked;
    if (CheckInDate && CheckOutDate) {
        console.log(CheckInDate, CheckOutDate, NumberOfGuests, Ordering, MinimumPrice, MaximumPrice, RoomC, FSuiteC, VSuiteC, HtlAprtmntC, VilaC);
        document.getElementById("SearchBtnSec2").click();
    }
}
document.addEventListener("DOMContentLoaded", () => {
    try {
        let sliders = document.querySelectorAll("#sliderMulti input");
        let updateSlider = (e) => {
            value = sliders[0].max - sliders[0].value;
            value2 = sliders[1].value;
            if (value > value2) {
                sliders[0].value = sliders[0].max - sliders[1].value;
                value = sliders[0].max - sliders[0].value;
                value2 = sliders[1].value;
            }
            document.getElementById("rangeViewer").innerText = `${value} EGP - ${value2} EGP`;
            document.getElementsByName("minPrice")[0].value = value;
            document.getElementsByName("maxPrice")[0].value = value2;
        }
        sliders[0].addEventListener("change", updateSlider);
        sliders[1].addEventListener("change", updateSlider);
        document.getElementById("decreaseGuestBtn").addEventListener("click", () => {
            if (document.getElementById("guestNLabel").value > 1) {
                document.getElementById("guestNLabel").value--;
            }
            GetFiltersHome();
        });
        document.getElementById("increaseGuestBtn").addEventListener("click", () => {
            document.getElementById("guestNLabel").value++;
            GetFiltersHome();
        });
        document.getElementById("SearchBtnSec1").addEventListener("click", () => {
            let dt1 = document.getElementById("CheckInDateT");
            let dt2 = document.getElementById("CheckOutDateT");
            let ng = document.getElementById("NumOfGuestsT");
            let CheckInDateTemp = dt1.value;
            let CheckOutDateTemp = dt2.value;
            if (CheckInDateTemp && CheckOutDateTemp) {
                let d1 = document.getElementById("CheckInDate");
                let d2 = document.getElementById("CheckOutDate");
                d1.type = "date";
                d2.type = "date";
                d1.value = CheckInDateTemp;
                d2.value = CheckOutDateTemp;
                document.getElementById("guestNLabel").value = ng.value;
                dt1.value = "";
                dt2.value = "";
                dt1.type = "text";
                dt2.type = "text";
                ng.value = 2;
                window.scrollTo(0, document.getElementById("BookingSection").offsetTop);
                GetFiltersHome();
            }
        });
        document.getElementById("CheckInDate").addEventListener("change", GetFiltersHome);
        document.getElementById("CheckOutDate").addEventListener("change", GetFiltersHome);
        document.getElementById("Sorting").addEventListener("change", GetFiltersHome);
        document.querySelectorAll("#sliderMulti input").forEach((e) => { e.addEventListener("change", GetFiltersHome); });
        document.getElementById("RoomCheckFilter").addEventListener("change", GetFiltersHome);
        document.getElementById("FSuiteCheckFilter").addEventListener("change", GetFiltersHome);
        document.getElementById("VSuiteCheckFilter").addEventListener("change", GetFiltersHome);
        document.getElementById("HtlAprtmntCheckFilter").addEventListener("change", GetFiltersHome);
        document.getElementById("VilaCheckFilter").addEventListener("change", GetFiltersHome);
    } catch (error) { }
    try {
        let redflagIncrease = document.getElementsByClassName("increaseOneRF");
        let redflagFields = document.getElementsByClassName("RFField");
        for (let i in redflagIncrease) {
            redflagIncrease[i].addEventListener("click", () => {
                redflagFields[i].innerText++;
            });
        }
    } catch (error) { }
});