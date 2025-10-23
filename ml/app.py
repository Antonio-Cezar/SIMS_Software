from fastapi import FastAPI
from pydantic import BaseModel
import pandas as pd
from sklearn.linear_model import LinearRegression

app = FastAPI()

class HistoryPoint(BaseModel):
    date: str
    quantity: float

class ForecastRequest(BaseModel):
    product_id: str
    history: list[HistoryPoint]
    horizon_days: int = 14

@app.post("/forecast")
def forecast(req: ForecastRequest):
    df = pd.DataFrame([{"ds": h.date, "y": h.quantity} for h in req.history])
    df["ds"] = pd.to_datetime(df["ds"])
    df = df.sort_values("ds")
    df["t"] = (df["ds"] - df["ds"].min()).dt.days
    model = LinearRegression().fit(df[["t"]], df["y"])
    return {"product_id": req.product_id, "forecast": "ok"}
