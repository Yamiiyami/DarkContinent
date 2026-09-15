# Tài liệu thiết kế hệ thống chỉ số Pet (Virtual Pet Stats System)

## 1. Tổng quan

Hệ thống mô phỏng một pet ảo với các chỉ số sống động, thay đổi theo **thời gian thực bên ngoài** và theo **hành động của người chơi**. Mục tiêu là tạo vòng lặp chăm sóc (cho ăn, cho ngủ, chơi hoạt động) có rủi ro và phần thưởng rõ ràng.

### Danh sách chỉ số

| Chỉ số | Ký hiệu | Khoảng giá trị | Loại |
|---|---|---|---|
| Tuổi (Age) | `age` | 0 → vô hạn | Tăng dần theo thời gian thực |
| Độ no (Hunger) | `hunger` | 0 – 100 | Giảm theo thời gian + hoạt động |
| Năng lượng (Energy) | `energy` | 0 – 100 | Giảm theo hoạt động (+ tự nhiên), hồi khi ngủ/nghỉ |
| Hạnh phúc (Happiness) | `happiness` | 0 – 100 | Tăng khi chơi, giảm khi đói/không tương tác |
| Cấp độ (Level) | `level` | 1 → vô hạn | Tăng khi đủ exp |
| Kinh nghiệm (Exp) | `exp` | 0 → theo mốc cấp | Tăng khi chơi hoạt động / ăn / ngủ |

---

## 2. Chi tiết từng chỉ số

### 2.1. Tuổi (Age)

- **Công thức**: `age_hours_real = 5` → mỗi 5 giờ thực = 1 tuổi.
- **Giai đoạn phát triển** (ảnh hưởng ngoại hình + hệ số chỉ số):

| Giai đoạn | Tuổi | Đặc điểm |
|---|---|---|
| Baby | 0 – 5 | Độ no/năng lượng giảm nhanh hơn (x1.3), nhận exp nhiều hơn (x1.2) |
| Teen | 6 – 15 | Hệ số chuẩn (x1.0) |
| Adult | 16+ | Ổn định, giảm chậm hơn (x0.8), mở khóa nhiều hoạt động |

- **Tác động**: đổi hình ảnh/animation pet theo giai đoạn; đổi hệ số tăng/giảm các chỉ số khác; mở khóa hoạt động theo giai đoạn.

---

### 2.2. Độ no (Hunger)

- **Giảm tự nhiên**: `-5 điểm / giờ thực`.
- **Giảm khi chơi hoạt động**: tùy loại hoạt động (xem bảng mục 4).
- **Tăng khi cho ăn**:
  - Mỗi loại thức ăn có giá trị hồi riêng (VD: thức ăn thường +20, thức ăn ngon +40, đồ ăn vặt +10).
  - **Cooldown cho ăn**: 15 phút thực giữa 2 lần ăn (tránh spam).
  - **Bội thực**: nếu cho ăn khi `hunger > 90`, hiệu quả hồi giảm 50% và trừ `-5 happiness` ("pet bị đầy bụng, không vui").
- **Hệ quả khi `hunger = 0` kéo dài**:
  - Sau mỗi giờ ở mức 0: `-10 energy`, `-5 happiness`.
  - Sau 3 giờ liên tục ở mức 0: chuyển trạng thái **"Bệnh"** (xem mục 5).

---

### 2.3. Năng lượng (Energy)

- **Giảm tự nhiên**: `-3 điểm / giờ thực` (pet luôn tiêu hao năng lượng cơ bản dù không làm gì).
- **Giảm khi chơi hoạt động**: tùy loại (mục 4), chỉ áp dụng cho hoạt động thể thao/vận động.
- **Hồi năng lượng**:
  - Ngủ (`sleep`): `+10 điểm / giờ thực` trong lúc ngủ.
  - Nghỉ ngơi thức (`rest`, không phải ngủ): `+5 điểm / giờ thực`.
- **Ràng buộc**: khi `energy = 0` → **không thể** bắt đầu hoạt động thuộc nhóm "Thể thao / Vận động". Vẫn có thể thực hiện hoạt động tĩnh (học tập, giải trí nhẹ) hoặc phải cho ngủ/nghỉ.
- **Hệ quả khi `energy = 0` kéo dài**: `-2 happiness / giờ`.

---

### 2.4. Hạnh phúc (Happiness)

- **Tăng khi chơi hoạt động**: tùy loại (mục 4).
- **Giảm khi đói**: nếu `hunger < 45` → `-3 happiness / giờ`.
- **Giảm khi năng lượng cạn**: nếu `energy = 0` kéo dài → `-2 happiness / giờ`.
- **Decay tự nhiên do thiếu tương tác**: nếu không có hoạt động nào được thực hiện trong `>= 3 giờ` thực → `-1 happiness / giờ` tính từ giờ thứ 3.
- **Hệ quả khi `happiness = 0` kéo dài**:
  - Giảm 30% exp nhận được từ mọi nguồn.
  - Hiển thị trạng thái cảm xúc "Buồn/Trầm cảm" trên UI.

---

### 2.5. Cấp độ & Kinh nghiệm (Level & Exp)

- **Công thức lên cấp** (lũy tiến, tránh cày quá dễ ở cấp cao):

```
exp_required(level) = 50 * level ^ 1.5
```

| Level | Exp cần (tích lũy tới cấp kế) |
|---|---|
| 1 → 2 | 50 |
| 2 → 3 | ~141 |
| 3 → 4 | ~260 |
| 4 → 5 | ~400 |

- **Nguồn exp**:
  - Hoạt động: tùy loại (mục 4).
  - Cho ăn: `+2 exp` (nhỏ, không phải nguồn cày chính).
  - Ngủ đủ giấc (dậy tự nhiên, không bị ngắt): `+5 exp`.
- **Phần thưởng khi lên cấp**:
  - Mỗi 5 cấp: `+10 max energy`, `+10 max hunger` (nới giới hạn chăm sóc).
  - Mốc cấp cụ thể mở khóa hoạt động mới (VD: cấp 5 → "Bơi", cấp 10 → "Chạy marathon").
  - Mở khóa trang phục/phụ kiện trang trí theo cấp.
  - Tăng tốc độ hồi năng lượng khi ngủ (+1 điểm/giờ mỗi 10 cấp).

---

## 3. Trạng thái "Khủng hoảng" (Crisis States)

Đây là các trạng thái đặc biệt khi chỉ số chạm đáy kéo dài, tạo rủi ro thực sự cho gameplay.

| Trạng thái | Điều kiện kích hoạt | Hậu quả | Cách khắc phục |
|---|---|---|---|
| **Bệnh (Sick)** | `hunger = 0` liên tục ≥ 3 giờ | Giảm 50% exp nhận được từ mọi hoạt động; icon cảnh báo trên UI | Cho ăn liên tục để `hunger > 50` trong 2 giờ, hoặc dùng vật phẩm "thuốc" (nếu có hệ thống item) |
| **Kiệt sức (Exhausted)** | `energy = 0` | Không thể chơi hoạt động vận động | Ngủ hoặc nghỉ ngơi để hồi năng lượng |
| **Buồn/Trầm cảm (Depressed)** | `happiness = 0` kéo dài ≥ 2 giờ | Giảm 30% exp toàn bộ | Chơi hoạt động tăng happiness liên tục |

> Ghi chú: các trạng thái có thể cộng dồn (VD: vừa Bệnh vừa Kiệt sức) → cần xử lý ưu tiên hiển thị & cộng dồn hệ số giảm exp theo phép nhân (không cộng thẳng để tránh về âm).

---

## 4. Phân loại hoạt động (Activities)

| Nhóm hoạt động | Ví dụ | Tốn năng lượng | Tốn độ no | Tăng happiness | Exp |
|---|---|---|---|---|---|
| Thể thao / Vận động | Chạy, bơi, nhảy | Cao (15–25) | Có (5–10) | Cao (+15) | Cao (+20) |
| Học tập / Trí tuệ | Giải đố, học kỹ năng | Thấp (0–5) | Thấp (2–5) | Trung bình (+8) | Trung bình (+15) |
| Giải trí nhẹ | Chơi đồ chơi, xem hoạt hình | Rất thấp (0–3) | Thấp (2–3) | Trung bình (+10) | Thấp (+8) |
| Ngủ (Sleep) | Ngủ qua đêm | Hồi (+10/giờ) | Không tốn | Không đổi | +5 khi dậy đủ giấc |
| Nghỉ ngơi (Rest) | Ngồi nghỉ, thư giãn | Hồi (+5/giờ) | Không tốn | +2 (nhẹ) | Không |

- **Điều kiện chơi**: hoạt động "Thể thao/Vận động" yêu cầu `energy > 0` (và nên có ngưỡng tối thiểu, VD `energy >= 10`, để tránh chơi với 1 điểm năng lượng còn lại).
- **Cooldown hoạt động** (khuyến nghị thêm): mỗi hoạt động có thời gian hồi riêng (VD: 30 phút) để tránh spam hoạt động liên tục lấy exp.

---

## 5. Bảng tổng hợp tốc độ thay đổi tự nhiên (theo giờ thực)

| Chỉ số | Tốc độ tự nhiên | Điều kiện |
|---|---|---|
| Độ no | -5 / giờ | Luôn áp dụng |
| Năng lượng | -3 / giờ | Luôn áp dụng (trừ khi đang ngủ/nghỉ) |
| Năng lượng (khi ngủ) | +10 / giờ | Khi trạng thái = "sleeping" |
| Năng lượng (khi nghỉ) | +5 / giờ | Khi trạng thái = "resting" |
| Hạnh phúc | -3 / giờ | Khi `hunger < 45` |
| Hạnh phúc | -2 / giờ | Khi `energy = 0` |
| Hạnh phúc | -1 / giờ | Khi không tương tác ≥ 3 giờ liên tục |

> Các hiệu ứng giảm happiness nên **cộng dồn có giới hạn** (VD: tối đa -5/giờ tổng cộng) để tránh giảm quá nhanh khi nhiều điều kiện xảy ra cùng lúc.

---

## 6. Vòng đời xử lý (Processing Flow) — gợi ý kỹ thuật

1. **Job định kỳ (Scheduled Job/Cron)** chạy mỗi giờ thực (hoặc mỗi X phút để mượt hơn):
   - Tính tuổi mới dựa trên tổng thời gian tồn tại.
   - Áp dụng giảm tự nhiên: `hunger`, `energy`.
   - Kiểm tra điều kiện kích hoạt trạng thái khủng hoảng (Bệnh, Kiệt sức, Buồn).
   - Áp dụng giảm/tăng `happiness` theo các điều kiện ở mục 5.
2. **Khi người chơi thực hiện hành động** (cho ăn / chơi hoạt động / ngủ):
   - Validate điều kiện (cooldown, năng lượng đủ, không bội thực...).
   - Cập nhật chỉ số tương ứng theo bảng mục 2 & 4.
   - Cộng exp, kiểm tra lên cấp (so sánh với `exp_required(level)`).
   - Ghi log hành động (phục vụ thống kê, chống spam, và feature "lịch sử chăm sóc").
3. **Khi lên cấp**:
   - Cập nhật max stats nếu đạt mốc.
   - Mở khóa hoạt động/trang phục nếu đạt mốc cấp quy định.

---

## 7. Vấn đề còn mở (cần quyết định thêm trước khi code)

- [ ] Có hệ thống **item/thuốc** để chữa bệnh nhanh không, hay chỉ chữa bằng cách chăm sóc thủ công?
- [ ] Pet có thể **"chết"** nếu bỏ bê quá lâu không, hay chỉ dừng ở trạng thái Bệnh/Buồn (không có game over)?
- [ ] Cooldown cụ thể cho từng hoạt động là bao nhiêu? (đang để trống, cần số liệu thật)
- [ ] Giới hạn tối đa các chỉ số có tăng vô hạn theo cấp độ không, hay dừng ở một mốc trần (VD: max 150)?
- [ ] Xử lý khi app/game bị tắt lâu (offline) — có tính dồn thời gian offline để trừ chỉ số khi mở lại không, và có giới hạn số giờ offline tính tối đa không (tránh pet "chết" oan khi user đi xa nhiều ngày)?

---

*Tài liệu này là bản thiết kế logic (game design doc), chưa bao gồm schema database hay code triển khai. Bước tiếp theo có thể là thiết kế schema (`pets`, `pet_stats`, `activities`, `activity_logs`, `food_items`...) theo kiến trúc Model → Interface → Repository → Service → Controller.*