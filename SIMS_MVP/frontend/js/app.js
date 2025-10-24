// ---- Global config ----
export const API = "http://localhost:5292"; // change to your API port

// ---- Set active sidebar link based on page ----
export function setActiveNav() {
  const path = location.pathname.split("/").pop() || "dashboard.html";
  document.querySelectorAll(".nav a").forEach(a => {
    a.classList.toggle("active", a.getAttribute("href") === path);
  });
}

// ---- Tiny sparkline chart (SVG) ----
export function renderSparkline(elId){
  const el = document.getElementById(elId);
  if(!el) return;
  const points = [10,30,22,40,28,55,48,70];
  const w = 600, h = 220, pad = 18;
  const step = (w - pad*2) / (points.length - 1);
  const max = Math.max(...points), min = Math.min(...points);
  const mapY = (v) => h - pad - ((v - min) / (max - min)) * (h - pad*2);
  const mapX = (i) => pad + i * step;
  const d = points.map((p,i)=>`${i===0?"M":"L"} ${mapX(i)} ${mapY(p)}`).join(" ");
  el.innerHTML = `
    <defs>
      <linearGradient id="g" x1="0" x2="0" y1="0" y2="1">
        <stop offset="0%" stop-color="#7aa2ff" stop-opacity=".35"/>
        <stop offset="100%" stop-color="#7aa2ff" stop-opacity="0"/>
      </linearGradient>
    </defs>
    <path d="${d}" fill="none" stroke="#7aa2ff" stroke-width="3" />
    <path d="${d} L ${mapX(points.length-1)} ${h-pad} L ${pad} ${h-pad} Z" fill="url(#g)" opacity=".5"/>
  `;
}

// ---- Helpers ----
export const setText = (id,val)=>{ const el=document.getElementById(id); if(el) el.textContent=String(val); };
export const escapeHtml = s => String(s).replace(/[&<>"']/g, m => ({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#039;'}[m]));
