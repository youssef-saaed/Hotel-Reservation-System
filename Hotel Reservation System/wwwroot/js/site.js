let UpdateRoomsResults = async () => {
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
    let response = await fetch(`/FetchRooms/${CheckInDate}/${CheckOutDate}/${NumberOfGuests}/${MinimumPrice}/${MaximumPrice}/${Ordering}/${RoomC}/${FSuiteC}/${VSuiteC}/${HtlAprtmntC}/${VilaC}`);
    let json = await response.json();
    let cont = document.getElementById("roomsContainer");
    let types = ["Room", "Family Suite", "VIP Suite", "Hotel Apartment", "Vila"];
    let c = 0;
    let InDateS = CheckInDate.split("-");
    let OutDateS = CheckOutDate.split("-");
    let InDateO = new Date(parseInt(InDateS[0]), parseInt(InDateS[1]) - 1, parseInt(InDateS[2]));
    let OutDateO = new Date(parseInt(OutDateS[0]), parseInt(OutDateS[1]) - 1, parseInt(OutDateS[2]));
    let datediff = Math.ceil(Math.abs(OutDateO - InDateO) / (1000 * 60 * 60 * 24));
    cont.innerHTML = "";
    for (let i in json) {
        cont.innerHTML += `
        <div class="resBoxSec2 row mx-1 my-4">
                    <div class="col-2 d-flex flex-column justify-content-center">
                        <div class="row d-flex justify-content-center">
                            <div class="col-4">
                                <img src="./Media/person.webp">
                            </div>
                            <div class="col-4">
                                <img src="./Media/person.webp">
                            </div>
                        </div>
                        <div class="row mt-2">
                            <span class="text-center">${json[i]["capacity"]} Guests</span>
                        </div>
                    </div>
                    <div class="col d-flex align-items-center">
                        <span class="text-center m-auto">${json[i]["description"]}</span>
                    </div>
                    <div class="col-2 d-flex align-items-center">
                        <a href="#" class="btn detBtn" data-bs-toggle="modal" data-bs-target="#_${json[i]["floorNum"]}${json[i]["roomNum"]}">Room details</a>
                        <div class="modal fade" id="_${json[i]["floorNum"]}${json[i]["roomNum"]}" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered modal-lg">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h1 class="modal-title fs-5" id="exampleModalLabel">Details</h1>
                                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                    </div>
                                    <div class="modal-body">
                                        <p><span class="fw-bolder">Room Number: </span>${json[i]["roomNum"]}</p>
                                        <p><span class="fw-bolder">Floor Number: </span>${json[i]["floorNum"]}</p>
                                        <p><span class="fw-bolder">Room type: </span>${types[json[i]["type"] - 1]}</p>
                                        <p><span class="fw-bolder">Description: </span>${json[i]["description"]}</p>
                                        <p><span class="fw-bolder">Properties: </span>${json[i]["properties"]}</p>
                                        <p><span class="fw-bolder">Cost: </span>${json[i]["cost"]}</p>
                                        <p><span class="fw-bolder">Capacity: </span>${json[i]["capacity"]}</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-2 d-flex flex-column justify-content-center">
                        <div class="row">
                            <span class="text-center boldPrice">${datediff * json[i]["cost"]} EGP</span>
                        </div>
                        <div class="row mt-2">
                            <span class="text-center">${json[i]["cost"]} EGP per night</span>
                        </div>
                    </div>
                    <div class="col d-flex align-items-center">
                        <a href="/Book/${json[i]["roomNum"]}/${json[i]["floorNum"]}/${CheckInDate}/${CheckOutDate}/${json[i]["cost"]}" class="btn bookBtn">BOOK NOW</a>
                    </div>
                </div>
        `;
        c++;
    }
    if (!c) {
        cont.innerHTML += "<div class='row'><p class='text-center'>Nothing to show :(</p></div>";
    }
}
function GetFiltersHome()
{
    let CheckInDate = document.getElementById("CheckInDate").value;
    let CheckOutDate = document.getElementById("CheckOutDate").value;
    if (CheckInDate && CheckOutDate) {
        document.getElementById("SearchBtnSec2").click();
    }
}
document.addEventListener("DOMContentLoaded", () => {
    try {
        let sliders = document.querySelectorAll("#sliderMulti input");
        let updateSlider = (e) => {
            value = parseInt(sliders[0].max) - parseInt(sliders[0].value) + parseInt(sliders[0].min);
            value2 = parseInt(sliders[1].value);
            if (value > value2) {
                sliders[0].value = parseInt(sliders[0].max) - parseInt(sliders[1].value) + parseInt(sliders[0].min);
                value = parseInt(sliders[0].max) - parseInt(sliders[0].value) + parseInt(sliders[0].min);
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
        document.getElementById("SearchBtnSec2").addEventListener("click", UpdateRoomsResults);
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