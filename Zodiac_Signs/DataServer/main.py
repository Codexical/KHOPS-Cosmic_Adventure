import json
import os
from flask import Flask, request, jsonify

app = Flask(__name__)

DATA_FILE = "data.json"


def get_astrological_sign(month, day):
    if (month == 1 and day >= 20) or (month == 2 and day <= 18):
        return 0
    if (month == 2 and day >= 19) or (month == 3 and day <= 20):
        return 1
    if (month == 3 and day >= 21) or (month == 4 and day <= 19):
        return 2
    if (month == 4 and day >= 20) or (month == 5 and day <= 20):
        return 3
    if (month == 5 and day >= 21) or (month == 6 and day <= 21):
        return 4
    if (month == 6 and day >= 22) or (month == 7 and day <= 22):
        return 5
    if (month == 7 and day >= 23) or (month == 8 and day <= 22):
        return 6
    if (month == 8 and day >= 23) or (month == 9 and day <= 22):
        return 7
    if (month == 9 and day >= 23) or (month == 10 and day <= 23):
        return 8
    if (month == 10 and day >= 24) or (month == 11 and day <= 22):
        return 9
    if (month == 11 and day >= 23) or (month == 12 and day <= 21):
        return 10
    if (month == 12 and day >= 22) or (month == 1 and day <= 19):
        return 11
    return -1


def get_astronomical_constellation(month, day):
    if (month == 1 and day >= 20) or (month == 2 and day <= 16):
        return 0
    if (month == 2 and day >= 17) or (month == 3 and day <= 11):
        return 1
    if (month == 3 and day >= 12) or (month == 4 and day <= 18):
        return 2
    if (month == 4 and day >= 19) or (month == 5 and day <= 13):
        return 3
    if (month == 5 and day >= 14) or (month == 6 and day <= 21):
        return 4
    if (month == 6 and day >= 22) or (month == 7 and day <= 20):
        return 5
    if (month == 7 and day >= 21) or (month == 8 and day <= 10):
        return 6
    if (month == 8 and day >= 11) or (month == 9 and day <= 16):
        return 7
    if (month == 9 and day >= 17) or (month == 10 and day <= 30):
        return 8
    if (month == 10 and day >= 31) or (month == 11 and day <= 23):
        return 9
    if (month == 11 and day >= 24) or (month == 11 and day <= 29):
        return 10
    if (month == 11 and day >= 30) or (month == 12 and day <= 17):
        return 11
    if (month == 12 and day >= 18) or (month == 1 and day <= 19):
        return 12
    return -1


def initialize_data_file():
    initial_counts = {
        "ASTROLOGICAL": [0 for _ in range(12)],
        "ASTRONOMICAL": [0 for _ in range(13)],
    }
    with open(DATA_FILE, "w", encoding="utf-8") as f:
        json.dump(initial_counts, f, ensure_ascii=False, indent=4)
    return initial_counts


def read_data():
    if not os.path.exists(DATA_FILE):
        return initialize_data_file()
    try:
        with open(DATA_FILE, "r", encoding="utf-8") as f:
            return json.load(f)
    except (json.JSONDecodeError, FileNotFoundError):
        return initialize_data_file()


def write_data(data):
    with open(DATA_FILE, "w", encoding="utf-8") as f:
        json.dump(data, f, ensure_ascii=False, indent=4)


@app.route("/")
def index():
    return "Zodiac Sign API is running."


@app.route("/record", methods=["POST"])
def record_constellation():
    try:
        req_data = request.get_json()
        if not req_data:
            return jsonify({"error": "Request body must be JSON"}), 400

        era = req_data.get("era")
        year = int(req_data.get("year"))
        month = int(req_data.get("month"))
        day = int(req_data.get("day"))

        if not all([era, year, month, day]):
            return jsonify({"error": "Missing required fields"}), 400

    except (ValueError, TypeError):
        return jsonify({"error": "Invalid input data"}), 400

    if era == "0":
        year += 1911

    astrological_index = get_astrological_sign(month, day)
    astronomical_index = get_astronomical_constellation(month, day)

    all_counts = read_data()
    all_counts["ASTROLOGICAL"][astrological_index] += 1
    all_counts["ASTRONOMICAL"][astronomical_index] += 1
    write_data(all_counts)

    response_data = {
        "astrological_index": astrological_index,
        "astronomical_index": astronomical_index,
    }

    return jsonify(response_data), 200


@app.route("/counts", methods=["GET"])
def get_all_counts():
    all_counts = read_data()
    return jsonify(all_counts), 200


@app.route("/clear", methods=["POST"])
def clear_all_counts():
    initialize_data_file()
    return jsonify({"message": "All counts have been reset."}), 200


if __name__ == "__main__":
    if not os.path.exists(DATA_FILE):
        initialize_data_file()
    app.run(host="0.0.0.0", port=5000, debug=True)
