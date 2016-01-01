#include <algorithm>
#include <bitset>
#include <fstream>
#include <iostream>
#include <map>
#include <queue>
#include <set>
#include <string>
#include <unordered_map>
#include <vector>

using namespace std;

inline unsigned int zipRGB(unsigned char r, unsigned char g, unsigned char b) {
  return r << 16 | g << 8 | b;
}

inline void unzipRGB(unsigned int rgb, unsigned char& r, unsigned char& g,
                     unsigned char& b) {
  r = (rgb >> 16) & 0xFF;
  g = (rgb >> 8) & 0xFF;
  b = (rgb >> 0) & 0xFF;
}

unordered_map<unsigned int, tuple<double, double, double> > lab_memo;
inline tuple<double, double, double> RGBtoLAB(const unsigned int rgb) {
  auto it = lab_memo.find(rgb);
  if (it != lab_memo.end()) {
    return it->second;
  }
  unsigned char r, g, b;
  unzipRGB(rgb, r, g, b);

  double var_R = (double)r;
  double var_G = (double)g;
  double var_B = (double)b;

  if (var_R > 0.04045)
    var_R = pow(((var_R + 0.055) / 1.055), 2.4);
  else
    var_R = var_R / 12.92;
  if (var_G > 0.04045)
    var_G = pow(((var_G + 0.055) / 1.055), 2.4);
  else
    var_G = var_G / 12.92;
  if (var_B > 0.04045)
    var_B = pow(((var_B + 0.055) / 1.055), 2.4);
  else
    var_B = var_B / 12.92;

  var_R = var_R * 100;
  var_G = var_G * 100;
  var_B = var_B * 100;

  double X = var_R * 0.4124 + var_G * 0.3576 + var_B * 0.1805;
  double Y = var_R * 0.2126 + var_G * 0.7152 + var_B * 0.0722;
  double Z = var_R * 0.0193 + var_G * 0.1192 + var_B * 0.9505;

  if (X > 0.008856)
    X = pow(X, (1.0 / 3));
  else
    X = (7.787 * X) + (16.0 / 116);
  if (Y > 0.008856)
    Y = pow(Y, (1.0 / 3));
  else
    Y = (7.787 * Y) + (16.0 / 116);
  if (Z > 0.008856)
    Z = pow(Z, (1.0 / 3));
  else
    Z = (7.787 * Z) + (16.0 / 116);

  const double CIE_L = (116 * Y) - 16;
  const double CIE_a = 500 * (X - Y);
  const double CIE_b = 200 * (Y - Z);
  return lab_memo[rgb] = make_tuple(CIE_L, CIE_a, CIE_b);
}

class PositionSolver {
 public:
  struct Piece {
    vector<vector<int> > line;
    vector<vector<double> > diff, _diff;
    vector<vector<int> > diff_rank;
  };

  struct Node {
    double diff;
    bitset<256> used_mask;
    vector<tuple<int, int> > id_pos;
    vector<int> log;
  };

 private:
  const vector<int> dx = {1, 0, -1, 0};
  const vector<int> dy = {0, 1, 0, -1};

  vector<Piece> piece_list;
  vector<vector<int> > rgb;
  set<tuple<int, int, int> > not_neighbor_set;
  int sep_x, sep_y;
  int N;
  int TW;
  int TH;
  int SW;
  int SH;

  void shift_position(Node& node) {
    int min_x = 1 << 30;
    int min_y = 1 << 30;
    for (int j = 0; j < N; ++j) {
      if (node.used_mask[j]) {
        min_x = min(min_x, get<0>(node.id_pos[j]));
        min_y = min(min_y, get<1>(node.id_pos[j]));
      }
    }
    for (int j = 0; j < N; ++j) {
      if (node.used_mask[j]) {
        get<0>(node.id_pos[j]) -= min_x;
        get<1>(node.id_pos[j]) -= min_y;
      }
    }
  }

  void make_piece_list() {
    piece_list.clear();
    piece_list.resize(N);
    for (int i = 0; i < N; ++i) {
      Piece& p = piece_list[i];
      p.line.resize(4);

      vector<vector<int> > img(SH, vector<int>(SW));
      const int bx = (i % sep_x) * SW;
      const int by = (i / sep_x) * SH;
      for (int y = 0; y < SH; ++y) {
        for (int x = 0; x < SW; ++x) {
          img[y][x] = rgb[by + y][bx + x];
        }
      }
      for (int y = 0; y < SH; ++y) {
        p.line[0].push_back(img[y][SW - 1]);  // R
        p.line[2].push_back(img[y][0]);       // L
      }
      for (int x = 0; x < SW; ++x) {
        p.line[1].push_back(img[SH - 1][x]);  // D
        p.line[3].push_back(img[0][x]);       // U
      }
    }
  }

  void calc_diff() {
    // 全ピース辺間のdiffを計算する
    for (int i = 0; i < N; ++i) {
      Piece& p = piece_list[i];
      p.diff = vector<vector<double> >(4, vector<double>(N));
      p.diff_rank = vector<vector<int> >(4, vector<int>(N));
    }

    for (int i = 0; i < N; ++i) {
      Piece& p = piece_list[i];
      for (int r = 0; r < 4; ++r) {
        p.diff[r][i] = 1e200;
        for (int j = 0; j < N; ++j) {
          if (i != j) {
            Piece& q = piece_list[j];
            int n = (int)p.line[r].size();
            for (int k = 0; k < n; ++k) {
              auto lab1 = RGBtoLAB(p.line[r][k]);
              auto lab2 = RGBtoLAB(q.line[(r + 2) % 4][k]);
              double dl = get<0>(lab1) - get<0>(lab2);
              double da = get<1>(lab1) - get<1>(lab2);
              double db = get<2>(lab1) - get<2>(lab2);

              p.diff[r][j] += sqrt(dl * dl + da * da + db * db);
            }
            p.diff[r][j] /= double(n);
          }
        }

        vector<pair<double, int> > diff_id(N);
        for (int j = 0; j < N; ++j) {
          diff_id[j] = {p.diff[r][j], j};
        }
        sort(diff_id.begin(), diff_id.end());
        for (int j = 0; j < N; ++j) {
          p.diff_rank[r][j] = diff_id[j].second;
        }
      }
    }

    for (int i = 0; i < N; ++i) {
      Piece& p = piece_list[i];
      p._diff = p.diff;
    }
  }

  map<tuple<int, int>, int> get_used_pos_map(const Node& node) {
    map<tuple<int, int>, int> pos_map;
    for (int i = 0; i < N; ++i) {
      if (node.used_mask[i]) {
        pos_map[node.id_pos[i]] = i;
      }
    }
    return pos_map;
  }

  tuple<int, int> get_boundingbox(const Node& node) {
    int min_x = 1 << 30;
    int min_y = 1 << 30;
    int max_x = 0;
    int max_y = 0;
    for (int i = 0; i < N; ++i) {
      if (node.used_mask[i]) {
        min_x = min(min_x, get<0>(node.id_pos[i]));
        min_y = min(min_y, get<1>(node.id_pos[i]));
        max_x = max(max_x, get<0>(node.id_pos[i]));
        max_y = max(max_y, get<1>(node.id_pos[i]));
      }
    }
    return make_tuple(max_x - min_x + 1, max_y - min_y + 1);
  }

  set<int> divide_into_segments(const Node& node) {
    set<set<int> > segment_set;
    auto used_pos = get_used_pos_map(node);

    for (int id = 0; id < N; ++id) {
      vector<bool> is_same_segment(N, false);
      is_same_segment[id] = true;
      queue<int> Q;
      Q.push(id);
      while (!Q.empty()) {
        int i = Q.front();
        Q.pop();
        for (int r = 0; r < 4; ++r) {
          tuple<int, int> rpos = make_tuple(get<0>(node.id_pos[i]) + dx[r],
                                            get<1>(node.id_pos[i]) + dy[r]);
          const auto it = used_pos.find(rpos);
          if (it == used_pos.end()) {
            continue;
          }

          const int j = it->second;
          if (!node.used_mask[j] || is_same_segment[j]) {
            continue;
          }

          bool agree = true;
          for (int qr = 0; qr < 4; ++qr) {
            tuple<int, int> qrpos = make_tuple(get<0>(node.id_pos[j]) + dx[qr],
                                               get<1>(node.id_pos[j]) + dy[qr]);
            const auto qit = used_pos.find(qrpos);
            if (qit == used_pos.end()) {
              continue;
            }
            const int k = qit->second;
            if (is_same_segment[k]) {
              agree &= k == piece_list[j].diff_rank[qr][0];
              agree &= j == piece_list[k].diff_rank[(qr + 2) % 4][0];
            }
          }
          if (agree) {
            is_same_segment[j] = true;
            Q.push(j);
          }
        }
      }

      set<int> segment;
      for (int i = 0; i < N; ++i) {
        if (is_same_segment[i]) {
          segment.insert(i);
        }
      }
      segment_set.insert(segment);
    }

    set<int> max_segment;
    for (auto& segment : segment_set) {
      if (max_segment.size() < segment.size()) {
        max_segment = segment;
      }
    }
    return max_segment;
  }

  vector<tuple<int, int> > get_empty_neighbors(
      const Node& node, const map<tuple<int, int>, int>& used) {
    auto box = get_boundingbox(node);
    vector<tuple<int, int, int, int> > cnt_pos;
    for (int i = 0; i < N; ++i) {
      if (node.used_mask[i]) {
        for (int r = 0; r < 4; ++r) {
          const int x = get<0>(node.id_pos[i]) + dx[r];
          const int y = get<1>(node.id_pos[i]) + dy[r];
          if (used.find(make_tuple(x, y)) == used.end()) {
            bool extendable = true;
            if (x == -1 || x == sep_x) {
              extendable &= (get<0>(box) != sep_x);
            }
            if (y == -1 || y == sep_y) {
              extendable &= (get<1>(box) != sep_y);
            }
            if (extendable) {
              cnt_pos.emplace_back(0, 0, x, y);
            }
          }
        }
      }
    }
    sort(cnt_pos.begin(), cnt_pos.end());
    cnt_pos.erase(unique(cnt_pos.begin(), cnt_pos.end()), cnt_pos.end());

    set<tuple<int, int> > pos_set;
    for (auto& n : cnt_pos) {
      pos_set.emplace(get<2>(n), get<3>(n));
    }

    for (auto& n : cnt_pos) {
      for (int r = 0; r < 4; ++r) {
        tuple<int, int> rpos = make_tuple(get<2>(n) + dx[r], get<3>(n) + dy[r]);
        if (used.find(rpos) != used.end()) {
          get<0>(n)++;
        }
        if (pos_set.find(rpos) != pos_set.end()) {
          get<1>(n)++;
        }
      }
    }
    sort(cnt_pos.rbegin(), cnt_pos.rend());
    vector<tuple<int, int> > ret;
    if (!cnt_pos.empty()) {
      int max_cnt = get<0>(cnt_pos.front());
      // int max_pos = get<1>(cnt_pos.front());
      for (const auto& p : cnt_pos) {
        if (get<0>(p) == max_cnt /* && get<1>(p) == max_pos */) {
          ret.emplace_back(get<2>(p), get<3>(p));
        }
      }
    }
    return ret;
  }

  vector<pair<pair<int, double>, pair<int, pair<int, int> > > >
  get_partner_info(const Node& node, const map<tuple<int, int>, int>& used,
                   const vector<tuple<int, int> >& empty_neighbors) {
    vector<pair<pair<int, double>, pair<int, pair<int, int> > > > partnerinfo;
    for (auto& neighbor : empty_neighbors) {
      int x = get<0>(neighbor);
      int y = get<1>(neighbor);

      // その周り4マスで確定しているピースID
      vector<int> around(4, -1);
      for (int r = 0; r < 4; ++r) {
        auto it = used.find(make_tuple(x + dx[r], y + dy[r]));
        if (it != used.end()) {
          around[r] = it->second;
        }
      }

      // iを置いた時のdiff平均値を計算
      // iを置いた時のpartnarの数を計算
      for (int i = 0; i < N; ++i) {
        if (!node.used_mask[i]) {
          int partner = 0;
          double cnt = 0;
          double diff = 0;
          for (int r = 0; r < 4; ++r) {
            if (around[r] != -1) {
              int j = around[r];
              diff += piece_list[i].diff[r][j];
              cnt++;
              if ((i == piece_list[j].diff_rank[(r + 2) % 4][0]) &&
                  (j == piece_list[i].diff_rank[r][0])) {
                partner++;
              }
            }
          }
          diff /= cnt;
          partnerinfo.push_back({{partner, -diff}, {i, {x, y}}});
        }
      }
    }
    return partnerinfo;
  }

  Node calc_permutation(Node node) {
    while ((int)node.used_mask.count() < N) {
      auto used = get_used_pos_map(node);
      auto empty_neighbors = get_empty_neighbors(node, used);
      auto partnerinfo = get_partner_info(node, used, empty_neighbors);

      vector<pair<double, tuple<int, int, int> > > next;

      for (auto& info : partnerinfo) {
        const int piece_id = info.second.first;
        const int x = info.second.second.first;
        const int y = info.second.second.second;
        const int partner = info.first.first;
        const double diff = info.first.second;

        double score = 1e8 * partner + 1e2 * diff;

        if (partner == 1) {
          // piece_id を (x, y) に配置してみて、その後の盤面での評価値を調べる
          Node tmp_node = node;
          tmp_node.used_mask.set(piece_id);
          tmp_node.id_pos[piece_id] = make_tuple(x, y);
          shift_position(tmp_node);
          auto tmp_used = used;
          tmp_used[make_tuple(x, y)] = piece_id;
          vector<tuple<int, int> > tmp_empty_neighbors;
          for (int r = 0; r < 4; ++r) {
            auto rpos = make_tuple(x + dx[r], y + dy[r]);
            auto it = tmp_used.find(rpos);
            if (it == tmp_used.end()) {
              tmp_empty_neighbors.push_back(rpos);
            }
          }
          auto tmp_partnerinfo =
              get_partner_info(tmp_node, tmp_used, tmp_empty_neighbors);
          if (!tmp_partnerinfo.empty()) {
            auto max_node =
                *max_element(tmp_partnerinfo.rbegin(), tmp_partnerinfo.rend());
            score += 1e6 * max_node.first.first + max_node.first.second;
          }
        }

        next.push_back(make_pair(score, make_tuple(piece_id, x, y)));
      }

      const auto decided = max_element(next.begin(), next.end())->second;
      const int i = get<0>(decided);
      const int x = get<1>(decided);
      const int y = get<2>(decided);
      node.used_mask[i] = true;
      node.id_pos[i] = make_tuple(x, y);
      node.log.push_back(i);
      shift_position(node);
    }
    return node;
  }

  void update_diff_rank() {
    vector<pair<double, int> > diff_id(N);
    for (int i = 0; i < N; ++i) {
      Piece& p = piece_list[i];
      for (int r = 0; r < 4; ++r) {
        for (int j = 0; j < N; ++j) {
          diff_id[j] = {p.diff[r][j], j};
        }
        sort(diff_id.begin(), diff_id.end());
        for (int j = 0; j < N; ++j) {
          p.diff_rank[r][j] = diff_id[j].second;
        }
      }
    }
  }

  void init() {
    make_piece_list();
    calc_diff();
  }

 public:
  void set_problem(const vector<vector<int> >& rgb, int sep_x, int sep_y) {
    this->sep_x = sep_x;
    this->sep_y = sep_y;
    this->N = sep_x * sep_y;
    this->TW = (int)rgb[0].size();
    this->TH = (int)rgb.size();
    this->SW = TW / sep_x;
    this->SH = TH / sep_y;
    this->rgb = rgb;
    init();
  }

  bool command_not_neighbor(int i, int j, int r) {
    if (i >= 0 && i < N && j >= 0 && j < N && r >= 0 && r < 4) {
      not_neighbor_set.emplace(i, j, r);
      not_neighbor_set.emplace(j, i, (r + 2) % 4);
      piece_list[i].diff[r][j] = 1e200;
      piece_list[j].diff[(r + 2) % 2][i] = 1e200;
      update_diff_rank();
      return true;
    }
    return false;
  }

  bool command_allow_neighbor(int i, int j, int r) {
    bool ok = true;
    ok &= not_neighbor_set.erase(make_tuple(i, j, r));
    ok &= not_neighbor_set.erase(make_tuple(j, i, (r + 2) % 4));
    if (ok) {
      piece_list[i].diff[r][j] = piece_list[i]._diff[r][j];
      piece_list[j].diff[(r + 2) % 2][i] = piece_list[j]._diff[(r + 2) % 4][i];
      update_diff_rank();
    }
    return ok;
  }

  bool command_not_neighbor_clear() {
    not_neighbor_set.clear();
    for (int i = 0; i < N; ++i) {
      piece_list[i].diff = piece_list[i]._diff;
    }
    update_diff_rank();
    return true;
  }

  tuple<set<tuple<int, int, int> > > command_get_config() {
    return make_tuple(not_neighbor_set);
  }

  vector<int> generate_permutation(const Node& node) {
    vector<tuple<int, int, int> > vt(N);
    for (int i = 0; i < N; ++i) {
      const int x = get<0>(node.id_pos[i]);
      const int y = get<1>(node.id_pos[i]);
      vt[i] = make_tuple(y, x, i);
    }
    sort(vt.begin(), vt.end());
    vector<int> ret(N);
    for (int i = 0; i < N; ++i) {
      ret[get<2>(vt[i])] = i;
    }
    return ret;
  }

  Node solve() {
    Node init_node;
    init_node.used_mask[0] = true;
    init_node.id_pos.resize(N);
    init_node.id_pos[0] = make_tuple(0, 0);
    auto ans = calc_permutation(init_node);
    int highest_segment_count = 0;
    int same_segment_count = 0;

    while (true) {
      auto max_segment = divide_into_segments(ans);
      if (highest_segment_count >= (int)max_segment.size()) {
        same_segment_count++;
        if (same_segment_count == 3) {
          break;
        }
      }
      highest_segment_count = (int)max_segment.size();
      ans.used_mask.reset();
      ans.log.clear();
      for (int i : max_segment) {
        ans.used_mask.set(i);
      }
      ans = calc_permutation(ans);
    }
    return ans;
  }
};

int main(int argc, const char* argv[]) {
  cerr << argv[0] << endl;
  cerr << argv[1] << endl;
  ifstream ifs(argv[1], ios::binary | ios::in);
  if (!ifs) return 0;

  bool is_intractive_mode = false;
  for (int i = 0; i < argc; ++i) {
    string arg = argv[i];
    if (arg == "-int") {
      is_intractive_mode = true;
    }
  }

  string comment;
  string tmp;
  vector<int> correct_permutation;
  int sep_x, sep_y;
  int select_limit;
  int select_cost_ratio, swap_cost_ratio;
  int width, height;
  int max_luminance;
  // P6
  // # 4 2 ········ 分割数
  // # 3 ··········· 選択可能回数
  // # 150 20 ····· コスト変換レート
  // # 正解順列(problem_makerの出力のみ)
  // 640 480 ······ ピクセル数
  // 255 ··········· 最大輝度
  // RGBRGBRGBRGBRGB...
  ifs >> comment;
  ifs >> comment >> sep_x >> sep_y;
  ifs >> comment >> select_limit;
  ifs >> comment >> select_cost_ratio >> swap_cost_ratio;
  ifs >> tmp;
  if (tmp == "#") {
    correct_permutation.resize(sep_x * sep_y);
    for (int i = 0; i < sep_x * sep_y; ++i) ifs >> correct_permutation[i];
    ifs >> width >> height;
  } else {
    width = atoi(tmp.c_str());
    ifs >> height;
  }
  ifs >> max_luminance;

  vector<vector<int> > input(height, vector<int>(width));

  for (char gomi = 0; gomi != '\n'; ifs.read(&gomi, sizeof(gomi)))
    ;

  for (int y = 0; y < height; ++y) {
    for (int x = 0; x < width; ++x) {
      unsigned char r, g, b;
      ifs.read((char*)&r, sizeof(r));
      ifs.read((char*)&g, sizeof(g));
      ifs.read((char*)&b, sizeof(b));
      input[y][x] = zipRGB(r, g, b);
    }
  }

  PositionSolver ps;
  ps.set_problem(input, sep_x, sep_y);

  PositionSolver::Node ans;
  if (is_intractive_mode) {
    while (cin.good()) {
      string command;
      cin >> command;
      if (command == "run") {
        cout << "OK" << endl;
        ans = ps.solve();
        auto perm = ps.generate_permutation(ans);
        cout << sep_x << " " << sep_y << endl;
        cout << select_limit << endl;
        cout << select_cost_ratio << " " << swap_cost_ratio << endl;
        for (int y = 0; y < sep_y; ++y) {
          for (int x = 0; x < sep_x; ++x) {
            cout << perm[x + y * sep_x] << " ";
          }
          cout << endl;
        }
        cout << "END" << endl;
      } else if (command == "bad") {
        int i, j, r;
        cin >> i >> j >> r;
        if (ps.command_not_neighbor(i, j, r)) {
          cout << "OK" << endl;
        } else {
          cout << "NG" << endl;
        }
        cout << "END" << endl;
      } else if (command == "allow") {
        int i, j, r;
        cin >> i >> j >> r;
        if (ps.command_allow_neighbor(i, j, r)) {
          cout << "OK" << endl;
        } else {
          cout << "NG" << endl;
        }
        cout << "END" << endl;
      } else if (command == "clear") {
        if (ps.command_not_neighbor_clear()) {
          cout << "OK" << endl;
        } else {
          cout << "NG" << endl;
        }
        cout << "END" << endl;
      } else if (command == "config") {
        cout << "OK" << endl;
        auto config = ps.command_get_config();
        auto bad_list = get<0>(config);
        for (auto& cond : bad_list) {
          cout << get<0>(cond) << " " << get<1>(cond) << " " << get<2>(cond)
               << endl;
        }
        cout << "END" << endl;
      } else if (command == "log") {
        cout << "OK" << endl;
        for (auto i : ans.log) {
          cout << i << endl;
        }
        cout << "END" << endl;
      } else {
        cout << "NG" << endl;
        cout << "END" << endl;
      }
    }
  } else {
    ans = ps.solve();
    auto perm = ps.generate_permutation(ans);
    cout << sep_x << " " << sep_y << endl;
    cout << select_limit << endl;
    cout << select_cost_ratio << " " << swap_cost_ratio << endl;
    for (int y = 0; y < sep_y; ++y) {
      for (int x = 0; x < sep_x; ++x) {
        cout << perm[x + y * sep_x] << " ";
      }
      cout << endl;
    }
  }
  return 0;
}
