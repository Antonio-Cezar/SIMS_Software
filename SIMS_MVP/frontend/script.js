// Bytt port hvis API’et ditt viser en annen
const API = "http://localhost:5292";

async function loadItems() {
  const res = await fetch(API + "/api/items");
  const data = await res.json();
  const body = document.getElementById("itemsBody");
  body.innerHTML = "";
  data.forEach(i => {
    const tr = document.createElement("tr");
    tr.innerHTML = `
      <td>${i.id}</td>
      <td><input value="${i.sku}" data-id="${i.id}" data-field="sku"/></td>
      <td><input value="${i.name}" data-id="${i.id}" data-field="name"/></td>
      <td><input type="number" value="${i.quantity}" data-id="${i.id}" data-field="quantity"/></td>
      <td>
        <button onclick="save(${i.id})">Lagre</button>
        <button onclick="removeItem(${i.id})">Slett</button>
      </td>`;
    body.appendChild(tr);
  });
}

async function save(id) {
  const rowInputs = [...document.querySelectorAll(`input[data-id="${id}"]`)];
  const payload = { sku: "", name: "", quantity: 0 };
  rowInputs.forEach(inp => payload[inp.dataset.field] =
    inp.type === "number" ? Number(inp.value) : inp.value);

  const res = await fetch(API + "/api/items/" + id, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload)
  });
  if (!res.ok) alert("Kunne ikke lagre");
  await loadItems();
}

async function removeItem(id) {
  if (!confirm("Slette varen?")) return;
  await fetch(API + "/api/items/" + id, { method: "DELETE" });
  await loadItems();
}

document.addEventListener("DOMContentLoaded", () => {
  document.getElementById("createForm").addEventListener("submit", async (e) => {
    e.preventDefault();
    const sku = document.getElementById("sku").value.trim();
    const name = document.getElementById("name").value.trim();
    const qty = Number(document.getElementById("qty").value || 0);

    const res = await fetch(API + "/api/items", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ sku, name, quantity: qty })
    });
    if (!res.ok) {
      const t = await res.text();
      alert("Feil: " + t);
      return;
    }
    e.target.reset();
    await loadItems();
  });

  loadItems();
});
