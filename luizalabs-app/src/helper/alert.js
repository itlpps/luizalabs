export default function showAlert(alertId, errorMessage) {
  const alert = document.querySelector("#" + alertId);
  alert.classList.remove("d-none");
  alert.innerHTML = errorMessage;
}
