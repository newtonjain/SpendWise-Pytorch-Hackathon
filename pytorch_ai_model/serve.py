import json
import os
import argparse

import torch
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from sklearn.externals import joblib
from flask import Flask, jsonify, request, render_template

from modules import Encoder, Decoder
from utils import numpy_to_tvar
import utils
from custom_types import TrainData
from constants import device

parser = argparse.ArgumentParser("SpendWise Predictor")
parser.add_argument("--debug", action="store_true", help="Enabled plots & "
                                                         "debug info",
                    default=False)
parser.add_argument("--num-samples", type=int, help="Number of samples to use"
                                                    "from default samples",
                    default=10)
parser.add_argument("--data-dir", default="data/Demo-User-Bank-Data.csv")
parser.add_argument("--out-seq-len", default=30, type=int)
parser.add_argument("--trained-model-dir", default="trained_models")
args = parser.parse_args()

# Initialize the app and set a secret_key.
app = Flask(__name__)
app.secret_key = 'something_secret'

def preprocess_data(dat, col_names, scale) -> TrainData:
    proc_dat = scale.transform(dat)

    mask = np.ones(proc_dat.shape[1], dtype=bool)
    dat_cols = list(dat.columns)
    for col_name in col_names:
        mask[dat_cols.index(col_name)] = False

    feats = proc_dat[:, mask]
    targs = proc_dat[:, ~mask]

    return TrainData(feats, targs)


def predict(encoder, decoder, t_dat, batch_size: int, T: int) -> np.ndarray:
    y_pred = np.zeros((t_dat.feats.shape[0] - T + 1, args.out_seq_len))

    for y_i in range(0, len(y_pred), batch_size):
        y_slc = slice(y_i, y_i + batch_size)
        batch_idx = range(len(y_pred))[y_slc]
        b_len = len(batch_idx)
        X = np.zeros((b_len, T - 1, t_dat.feats.shape[1]))
        y_history = np.zeros((b_len, T - 1, args.out_seq_len))
        # t_dat.targs.shape[0]))

        for b_i, b_idx in enumerate(batch_idx):
            idx = range(b_idx, b_idx + T - 1)

            X[b_i, :, :] = t_dat.feats[idx, :]
            y_history[b_i, :] = t_dat.targs[idx]

        y_history = numpy_to_tvar(y_history)
        _, input_encoded = encoder(numpy_to_tvar(X))
        y_pred[y_slc] = decoder(input_encoded, y_history).cpu().data.numpy()

    return y_pred

debug = args.debug
save_plots = True

def load_model(model_dir="trained_models", data_dir="data"):
    # Load the Encoder
    with open(os.path.join(data_dir, "enc_kwargs.json"), "r") as fi:
        enc_kwargs = json.load(fi)
    enc = Encoder(**enc_kwargs)
    enc.load_state_dict(torch.load(os.path.join(model_dir, "encoder.torch"),
                                   map_location=device))

    # Load the Decoder
    with open(os.path.join(data_dir, "dec_kwargs.json"), "r") as fi:
        dec_kwargs = json.load(fi)
    dec = Decoder(**dec_kwargs)
    dec.load_state_dict(torch.load(os.path.join(model_dir, "decoder.torch"),
                                   map_location=device))

    # Load DARNN params
    with open(os.path.join(data_dir, "da_rnn_kwargs.json"), "r") as fi:
        da_rnn_kwargs = json.load(fi)
    return enc, dec, da_rnn_kwargs


def load_data(data_dir="data", num_samples=10):
    raw_data = pd.read_csv(args.data_dir)
    # targ_cols = ("cat1", "cat2", "cat3", "cat4", "cat5")
    targ_cols = ("value", )
    scaler = joblib.load(os.path.join(data_dir, "scaler.pkl"))
    data = preprocess_data(raw_data.iloc[:num_samples, :], targ_cols, scaler)
    return data


def parse_inputs(request_dict, category_names):
    x_list = []
    for category in category_names:
        value = request_dict.get(category, None)
        if value:
            x_list.append(value)
    return x_list


@app.route('/api', methods=['GET', 'POST'])
def api():
    trained_model_dir = "trained_models"
    category_names = ["category1",]
    out_seq_len = 30
    if not request.json:
        return jsonify({"error": "Invalid inputs"})
    x_list = parse_inputs(request.json, category_names)
    input_data = pd.DataFrame(x_list)
    scaler = joblib.load(os.path.join(trained_model_dir, "scaler.pkl"))
    proc_data = scaler.transform(input_data)
    feats = proc_data
    targets = np.random.randint(1, 100, size=(1, out_seq_len))

    data = TrainData(feats, targets)
    print("data:", data)
    enc, dec, kwargs = load_model(trained_model_dir)
    pred = predict(enc, dec, data, **kwargs)
    results = jsonify({"predictions": np.squeeze(pred).tolist()})
    return results


if __name__ == "__main__":
    app.run()