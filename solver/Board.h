#pragma once
#include <cassert>
#include <sstream>

const int dx[] = {0, +1, 0, -1};
const int dy[] = {-1, 0, +1, 0};
const string dstr = "URDL";
string itos(int i) {
  stringstream ss;
  ss << i;
  return ss.str();
}

typedef array<int, 2> P;

class SwapProblem {
 public:
  int select_limit;
  int select_cost;
  int move_cost;
  int width;
  int height;
  vector<int> field;
};

class SwapAnswer {
  SwapProblem problem;
  vector<array<int, 2>> move_log;
  const string POS_STR = "0123456789ABCDEF";
  int select_count = 0;
  int move_count = 0;

 public:
  SwapAnswer(SwapProblem problem)
      : problem(problem), move_log(), select_count(0), move_count(0) {}

  SwapAnswer(const SwapAnswer& o)
      : problem(o.problem),
        move_log(o.move_log),
        select_count(o.select_count),
        move_count(o.move_count) {}

  SwapAnswer& operator=(const SwapAnswer& o) {
    if (this != &o) {
      problem = o.problem;
      move_log = o.move_log;
      select_count = o.select_count;
      move_count = o.move_count;
    }
    return *this;
  }

  void push(int from, int to) {
    if (move_log.empty() || move_log.back()[1] != from) {
      select_count++;
    }
    move_log.push_back({from, to});
    move_count++;
  }

  int estimate_cost() const {
    return select_count * problem.select_cost + move_count * problem.move_cost;
  }

  int get_move_count() const { return move_count; }

  int get_select_count() const { return select_count; }

  string tostring() const {
    stringstream ss;
    vector<vector<array<int, 2>>> fixed_log;
    for (int i = 0; i < move_log.size(); i++) {
      if (!i || move_log[i][0] != move_log[i - 1][1]) {
        fixed_log.push_back({});
      }
      fixed_log.back().push_back(move_log[i]);
    }
    string newline = "\n";
    ss << fixed_log.size() << newline;
    for (auto item : fixed_log) {
      ss << POS_STR[item[0][0] % problem.width]
         << POS_STR[item[0][0] / problem.width] << newline;
      ss << item.size() << newline;
      for (int i = 0; i < item.size(); i++) {
        int diff_x = item[i][1] % problem.width - item[i][0] % problem.width;
        int diff_y = item[i][1] / problem.width - item[i][0] / problem.width;
        for (int j = 0; j < 4; j++) {
          if (diff_x == dx[j] && diff_y == dy[j]) {
            ss << dstr[j];
          }
        }
      }
      ss << newline;
    }
    return ss.str();
  }
};

class Board {
  int _height, _width;
  vector<bool> locked;
  vector<int> _b;
  int rotated = 0;  // per 90[deg], clockwise
  int selected_index = 0;
  int remaining_select_count = 0;

 public:
  Board(int _width, int _height, int select_count, const vector<int>& _b)
      : _width(_width),
        _height(_height),
        _b(_b),
        locked(_width * _height),
        rotated(0),
        remaining_select_count(select_count) {}
  Board() { _height = _width = 0; }
  Board(const SwapProblem& prob)
      : _height(prob.height),
        _width(prob.width),
        _b(prob.field),
        locked(_width * _height),
        rotated(0),
        remaining_select_count(prob.select_limit) {}
  Board(const Board& o)
      : _height(o._height),
        _width(o._width),
        locked(o.locked),
        _b(o._b),
        rotated(o.rotated),
        selected_index(o.selected_index),
        remaining_select_count(o.remaining_select_count) {}

  int width() const { return rotated & 1 ? _height : _width; }
  int height() const { return rotated & 1 ? _width : _height; }
  int orgWidth() const { return _width; }
  int orgInd(int x, int y) const {
    switch (rotated) {
      case 1:
        swap(x, y);
        y = _height - 1 - y;
        break;
      case 2:
        x = _width - 1 - x;
        y = _height - 1 - y;
        break;
      case 3:
        swap(x, y);
        x = _width - 1 - x;
        break;
      default:
        break;
    }
    return y * _width + x;
  }

  P imgPos(int index) const {
    int x = index % _width;
    int y = index / _width;
    switch (rotated) {
      case 1:
        y = _height - 1 - y;
        swap(x, y);
        break;
      case 2:
        x = _width - 1 - x;
        y = _height - 1 - y;
        break;
      case 3:
        x = _width - 1 - x;
        swap(x, y);
        break;
      default:
        break;
    }
    return {x, y};
  }

  int ele(int x, int y) const { return _b[orgInd(x, y)]; }

  bool is_locked(int x, int y) const { return locked[orgInd(x, y)]; }

  void lock(int x, int y) { locked[orgInd(x, y)] = true; }

  void unlock(int x, int y) { locked[orgInd(x, y)] = false; }

  void select(int x, int y) {
    if (selected_index == orgInd(x, y)) {
      return;
    }
    remaining_select_count--;
    assert(remaining_select_count >= 0);
    selected_index = orgInd(x, y);
  }

  int get_remaining_select_count() { return remaining_select_count; }

  void move(int x, int y) {
    const int a = orgInd(x, y);
    swap(_b[a], _b[selected_index]);
    selected_index = a;
  }

  P get_selected() const { return imgPos(selected_index); }

  void rotate() {
    ++rotated;
    rotated &= 3;
  }

  int calc_move_distance(int x, int y) const {
    if (x == 0 && y == 0) return 0;
    x = abs(x);
    y = abs(y);
    int d = abs(x - y);
    int m = min(x, y);
    int ret = d * 5 + m * 6 - 4;
    if (x == y) ret += 2;
    return ret;
  }

  Board clone() { return (*this); }

  bool ok() {
    for (int i = 0; i < height(); i++)
      for (int j = 0; j < width(); j++)
        if (ele(j, i) != orgInd(j, i)) return false;
    return true;
  }
  P where(int index) {
    for (int i = 0; i < height(); i++) {
      for (int j = 0; j < width(); j++) {
        if (ele(j, i) == index) return {j, i};
      }
    }
    return {-1, -1};
  }
  P offset() {
    for (int i = 0; i < height(); i++) {
      for (int j = 0; j < width(); j++) {
        if (is_locked(j, i) == 0) return {j, i};
      }
    }
    return {0, 0};
  }
  int orgInd(const P& p) const { return orgInd(p[0], p[1]); }
  int ele(const P& p) const { return ele(p[0], p[1]); }
  bool is_locked(const P& p) const { return locked[orgInd(p)]; }
  void lock(const P& p) { locked[orgInd(p)] = true; }
  void unlock(const P& p) { locked[orgInd(p)] = false; }

  void init_debug() {
    _height = 16;
    _width = 16;
    for (int i = 0; i < _height * _width; i++) _b.push_back(i);
  }
  void shuffle() { random_shuffle(_b.begin(), _b.end()); }
  void debug() const {
    cerr << width() << " " << height() << endl;
    for (int i = 0; i < height(); i++) {
      for (int j = 0; j < width(); j++) {
        fprintf(stderr, "%4d", ele(j, i));

        if (orgInd(j, i) == selected_index) {
          fprintf(stderr, "+");
        } else if (is_locked(j, i)) {
          fprintf(stderr, "*");
        } else {
          fprintf(stderr, " ");
        }
      }
      cerr << endl;
    }
  }
};
struct Focus {
  P offset;
  int width, height;
  Focus(P offset, int width, int height)
      : offset(offset), width(width), height(height) {}
  bool in(P p) {
    if (offset[0] <= p[0] && p[0] < offset[0] + width && offset[1] <= p[1] &&
        p[1] < offset[1] + height)
      return true;
    else
      return false;
  }
};
