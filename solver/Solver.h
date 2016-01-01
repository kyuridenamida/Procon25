#pragma once
#include "kaga.h"
#include "Board.h"

class Solver {
  SwapProblem problem;
  SwapAnswer answer;

 public:
  Solver(SwapProblem problem) : problem(problem), answer(problem) {}

  Solver(const Solver &o) : problem(o.problem), answer(o.answer) {}

  bool is_valid_route(const vector<P> &seq) {
    return !seq.size() || seq[0][0] != -1;
  }

  vector<P> dijkstra(const Board &board, int sx, int sy, int gx, int gy) {
    if (board.is_locked(sx, sy)) return {{-1, -1}};  // error

    auto to1d = [](P p) -> int { return p[1] * 16 + p[0]; };
    auto to2d = [](int v) -> P { return {v % 16, v / 16}; };
    auto ok = [&](P p) -> bool {
      return !(p[0] < 0 || p[0] >= board.width() || p[1] < 0 ||
               p[1] >= board.height() || board.is_locked(p[0], p[1]));
    };
    auto swapEffect = [](const Board &board, P a, P b) -> int {
      const int OW = board.orgWidth();
      const int num = board.ele(b[0], b[1]);
      const int prev = board.orgInd(b[0], b[1]);
      const int next = board.orgInd(a[0], a[1]);
      int dist_prev =
          board.calc_move_distance(num % OW - prev % OW, num / OW - prev / OW);
      int dist_next =
          board.calc_move_distance(num % OW - next % OW, num / OW - next / OW);
      return 4 + dist_next - dist_prev;
    };
    array<int, 16 * 16> shortestCost;
    shortestCost.fill(1e9);
    array<int, 16 * 16> prev;
    prev.fill(-1);
    struct Node {
      P p;
      int cost;
      Node(P p, int cost) : p(p), cost(cost) {}
      bool operator<(const Node &x) const { return cost > x.cost; }
    };
    priority_queue<Node> Q;
    Q.push({{sx, sy}, 0});
    shortestCost[to1d({sx, sy})] = 0;
    while (Q.size()) {
      Node q = Q.top();
      Q.pop();
      if (shortestCost[to1d(q.p)] != q.cost) continue;
      if (q.p == P{gx, gy}) {
        vector<P> res = {};
        while (q.p != P{sx, sy}) {
          res.push_back(q.p);
          q.p = to2d(prev[to1d(q.p)]);
        }
        reverse(res.begin(), res.end());
        return res;
      }
      for (int i = 0; i < 4; i++) {
        P next{q.p[0] + dx[i], q.p[1] + dy[i]};
        if (!ok(next)) continue;
        int nextCost = q.cost + 1 + swapEffect(board, q.p, next);
        if (shortestCost[to1d(next)] <= nextCost) continue;
        shortestCost[to1d(next)] = nextCost;
        prev[to1d(next)] = to1d(q.p);
        Q.push({next, nextCost});
      }
    }
    return {{-1, -1}};  // failed
  };

  vector<P> nearMiniDijkstra(const Board &board, int sx, int sy, Focus focus) {
    if (board.is_locked(sx, sy)) return {{-1, -1}};  // error

    auto to1d = [](P p) -> int { return p[1] * 16 + p[0]; };
    auto to2d = [](int v) -> P { return {v % 16, v / 16}; };
    auto ok = [&](P p) -> bool {
      return !(p[0] < 0 || p[0] >= board.width() || p[1] < 0 ||
               p[1] >= board.height() || board.is_locked(p[0], p[1]));
    };
    auto swapEffect = [](const Board &board, P a, P b) -> int {
      const int OW = board.orgWidth();
      const int num = board.ele(b[0], b[1]);
      const int prev = board.orgInd(b[0], b[1]);
      const int next = board.orgInd(a[0], a[1]);
      int dist_prev =
          board.calc_move_distance(num % OW - prev % OW, num / OW - prev / OW);
      int dist_next =
          board.calc_move_distance(num % OW - next % OW, num / OW - next / OW);
      return 4 + dist_next - dist_prev;
    };
    array<int, 16 * 16> shortestCost;
    shortestCost.fill(1e9);
    array<int, 16 * 16> prev;
    prev.fill(-1);
    struct Node {
      P p;
      int cost;
      Node(P p, int cost) : p(p), cost(cost) {}
      bool operator<(const Node &x) const { return cost > x.cost; }
    };
    priority_queue<Node> Q;
    Q.push({{sx, sy}, 0});
    shortestCost[to1d({sx, sy})] = 0;
    while (Q.size()) {
      Node q = Q.top();
      Q.pop();
      if (shortestCost[to1d(q.p)] != q.cost) continue;
      if (focus.in(q.p)) {
        vector<P> res = {};
        while (q.p != P{sx, sy}) {
          res.push_back(q.p);
          q.p = to2d(prev[to1d(q.p)]);
        }
        reverse(res.begin(), res.end());
        return res;
      }
      for (int i = 0; i < 4; i++) {
        P next{q.p[0] + dx[i], q.p[1] + dy[i]};
        if (!ok(next)) continue;
        int nextCost = q.cost + 1 + swapEffect(board, q.p, next);
        if (shortestCost[to1d(next)] <= nextCost) continue;
        shortestCost[to1d(next)] = nextCost;
        prev[to1d(next)] = to1d(q.p);
        Q.push({next, nextCost});
      }
    }
    return {{-1, -1}};  // failed
  };
  vector<P> nearDo(Board board, int sx, int sy, Focus focus) {
    vector<P> ret;

    auto route = nearDijkstra(board, sx, sy, focus);

    if (!is_valid_route(route)) {
      return {{-1, -1}};
    }
    P pos = P{sx, sy};
    for (auto way : route) {
      auto emptyPos = board.get_selected();
      board.lock(pos[0], pos[1]);
      auto route2 = dijkstra(board, emptyPos[0], emptyPos[1], way[0], way[1]);

      for (auto way2 : route2) {
        board.move(way2[0], way2[1]);
        ret.push_back(way2);
      }
      board.unlock(pos[0], pos[1]);
      board.move(pos[0], pos[1]);
      ret.push_back(pos);
      pos = way;
    }
    return ret;
  }

  vector<P> ultra_dijkstra(const Board &board, int sx, int sy, int gx, int gy) {
    if (board.is_locked(sx, sy)) return {{-1, -1}};  // error
    const int W = board.width();
    const int H = board.height();

    struct Node {
      P p;
      Board b;
      int cost;
      Node(const Node &o) : p(o.p), b(o.b), cost(o.cost) {}
      Node(Board b, P p, int cost) : p(p), b(b), cost(cost) {}
      bool operator<(const Node &x) const { return cost > x.cost; }
    };

    const auto to1d = [](P p) -> int { return p[1] * 16 + p[0]; };
    const auto to2d = [](int v) -> P { return {v % 16, v / 16}; };
    const auto ok = [W, H](const Board &b, const P &p) -> bool {
      return !(p[0] < 0 || p[0] >= W || p[1] < 0 || p[1] >= H ||
               b.is_locked(p[0], p[1]));
    };
    const auto swapEffect = [](const Board &board, const P &a, const P &b)
                                -> int {
      // 空白セルが a から b に移動したとき,
      // ともなって動いたセルのコストを計算する
      // boardは移動前の状態であること
      const int OW = board.orgWidth();
      const int num = board.ele(b[0], b[1]);
      const int prev = board.orgInd(b[0], b[1]);
      const int next = board.orgInd(a[0], a[1]);
      int dist_prev =
          board.calc_move_distance(num % OW - prev % OW, num / OW - prev / OW);
      int dist_next =
          board.calc_move_distance(num % OW - next % OW, num / OW - next / OW);
      return 4 + dist_next - dist_prev;
    };

    struct SubDijkstraResult {
      vector<P> moves;
      int cost;
    };
    auto sub_dijkstra = [to1d, to2d, ok, swapEffect](const Board &board, int sx,
                                                     int sy, int gx, int gy)
                            -> SubDijkstraResult {
      array<int, 16 * 16> shortestCost;
      shortestCost.fill(1e9);
      array<int, 16 * 16> prev;
      prev.fill(-1);
      struct Node {
        P p;
        int cost;
        Node(P p, int cost) : p(p), cost(cost) {}
        bool operator<(const Node &x) const { return cost > x.cost; }
      };
      priority_queue<Node> Q;
      Q.emplace(P{sx, sy}, 0);
      shortestCost[to1d({sx, sy})] = 0;

      while (Q.size()) {
        Node q = Q.top();
        Q.pop();
        if (shortestCost[to1d(q.p)] != q.cost) continue;
        if (q.p == P{gx, gy}) {
          SubDijkstraResult res;
          res.cost = q.cost;
          while (q.p != P{sx, sy}) {
            res.moves.push_back(q.p);
            q.p = to2d(prev[to1d(q.p)]);
          }
          reverse(res.moves.begin(), res.moves.end());
          return res;
        }
        for (int i = 0; i < 4; i++) {
          const P next{q.p[0] + dx[i], q.p[1] + dy[i]};
          if (!ok(board, next)) continue;
          int nextCost = q.cost + 1 + swapEffect(board, q.p, next);
          if (shortestCost[to1d(next)] <= nextCost) continue;
          shortestCost[to1d(next)] = nextCost;
          prev[to1d(next)] = to1d(q.p);
          Q.emplace(next, nextCost);
        }
      }

      SubDijkstraResult bad;
      bad.cost = -1;
      return bad;
    };

    array<int, 16 * 16> shortestCost;
    shortestCost.fill(1e6);
    array<int, 16 * 16> prev;
    prev.fill(-1);
    priority_queue<Node> Q;
    Node node(board, {sx, sy}, 0);
    shortestCost[to1d({sx, sy})] = 0;
    Q.push(node);

    while (Q.size()) {
      Node q = Q.top();
      Q.pop();
      if (shortestCost[to1d(q.p)] != q.cost) continue;
      if (q.p == P{gx, gy}) {
        vector<P> res = {};
        while (q.p != P{sx, sy}) {
          res.push_back(q.p);
          q.p = to2d(prev[to1d(q.p)]);
        }
        reverse(res.begin(), res.end());
        return res;
      }

      q.b.lock(q.p[0], q.p[1]);  // 移動させたいピースの位置を固定
      for (int d = 0; d < 4; d++) {
        const P next_pos{q.p[0] + dx[d],
                         q.p[1] + dy[d]};  // 移動させたいピースの近傍
        if (!ok(q.b, next_pos)) continue;
        if (shortestCost[to1d(next_pos)] <= q.cost) continue;

        // 現在の空白マス -> 移動させたいピースの近傍へダイクストラ
        const P empty_pos = q.b.get_selected();
        auto submove = sub_dijkstra(q.b, empty_pos[0], empty_pos[1],
                                    next_pos[0], next_pos[1]);
        if (submove.cost != -1) {
          Node next_node = q;
          next_node.cost += submove.cost;
          for (const P &p : submove.moves) {
            next_node.b.move(p[0], p[1]);
          }

          // 移動させたいピースの近傍にいるはず
          next_node.cost += 1 + swapEffect(next_node.b, next_pos, q.p);

          // 移動させたいピースを１マス移動させたが
          // それよりも短いコストで同じ位置まで移動できる経路が存在した場合は無視
          if (shortestCost[to1d(next_pos)] <= next_node.cost) continue;
          shortestCost[to1d(next_pos)] = next_node.cost;

          // 空白セルを移動させたいピースと入れ替え、移動させたいピースを１マス移動する
          next_node.b.unlock(q.p[0], q.p[1]);
          next_node.b.move(q.p[0], q.p[1]);
          next_node.p = next_pos;
          prev[to1d(next_node.p)] = to1d(q.p);
          Q.emplace(next_node);
        }
      }
    }
    return {{-1, -1}};  // failed
  };

  P getPos(const Board &board, int id) {
    for (int i = 0; i < board.height(); i++) {
      for (int j = 0; j < board.width(); j++) {
        if (board.ele(j, i) == id) return {j, i};
      }
    }
    return {};
  };

  SwapAnswer solve(bool any_answer = false, bool use_select_limit = true) {
    Board current(problem);

    SwapAnswer best_answer(problem);
    do {
      for (int i = 0; i < problem.height; i++) {
        for (int j = 0; j < problem.width; j++) {
          if (KAGA::check(current, current.ele(j, i))) {
            Solver solver(*this);

            GreedySolverInfo info = solver.greedy2(current, j, i);

            if (best_answer.estimate_cost() == 0 ||
                best_answer.estimate_cost() > solver.answer.estimate_cost()) {
              best_answer = solver.answer;
              ofstream ofs("answer.txt");
              ofs << best_answer.tostring();
            }

            if (any_answer) {
              return best_answer;
            }

            // FIXME 使い方微妙
            if (use_select_limit && current.get_remaining_select_count() > 1) {
              const int target = info.worst_line_ans.worst_move_id;
              const P target_pos = current.imgPos(target);
              const P current_pos = getPos(current, target);
              const auto route =
                  dijkstra(current, current_pos[0], current_pos[1],
                           target_pos[0], target_pos[1]);
              current.select(current_pos[0], current_pos[1]);
              for (auto &way : route) {
                auto a = current.get_selected();
                answer.push(current.orgInd(a[0], a[1]),
                            current.orgInd(way[0], way[1]));
                current.move(way[0], way[1]);
              }
            }
          }
        }
      }
    } while (use_select_limit && current.get_remaining_select_count() > 1);
    return best_answer;
  }

  vector<pair<P, int>> get_next_target_positions(const Board &board) {
    // 1行取り出す
    bool done = false;
    vector<pair<P, int>> targets;
    for (int i = 0; i < board.height(); i++) {
      for (int j = 0; j < board.width(); j++) {
        if (!board.is_locked(j, i)) {
          for (int k = 0; k < board.width(); k++) {
            int id2 = board.orgInd(k, i);
            if (!board.is_locked(k, i)) targets.push_back({{k, i}, id2});
          }
          done = true;
          break;
        }
      }
      if (done) break;
    }
    return targets;
  }

  bool targets_are_already_completed(const Board &board,
                                     const vector<pair<P, int>> &targets) {
    // 完全に完成しているかどうかチェック
    for (auto item : targets) {
      if (board.ele(item.first[0], item.first[1]) !=
          board.orgInd(item.first[0], item.first[1]))
        return false;
    }
    return true;
  }

  bool targets_contain_selected_pos(const Board &board,
                                    const vector<pair<P, int>> &targets) {
    // 操作しているマスが含まれていないかチェック(移動に使うマスは最後にしたい)
    auto emptyPos = board.get_selected();
    for (auto item : targets) {
      if (item.second == board.ele(emptyPos[0], emptyPos[1])) {
        return true;
      }
    }
    return false;
  }

  vector<array<int, 2>> get_2x2_finalize_move(Board board) {
    int c = 0;
    bool stopFlag = false;
    for (int i = 0; i < board.height(); i++) {
      for (int j = 0; j < board.width(); j++) {
        if (!board.is_locked(j, i)) {
          auto emptyPos = board.get_selected();
          if (board.ele(emptyPos[0], emptyPos[1]) == board.ele(j, i)) {
            stopFlag = true;
            break;
          }
          c++;
        }
      }
      if (stopFlag) break;
    }
    //ひたすらくるくる回転する
    vector<array<int, 2>> ret;
    const string kurukuru_table[] = {"RDLU", "DLUR", "URDL", "LURD"};
    for (int i = 0; !board.ok(); i++) {
      string str = string(1, kurukuru_table[c][i % kurukuru_table[c].size()]);
      for (char dir : str) {
        auto d = dstr.find(dir);
        auto emptyPos = board.get_selected();
        auto movefrom = board.orgInd(emptyPos[0], emptyPos[1]);
        auto moveto = board.orgInd(emptyPos[0] + dx[d], emptyPos[1] + dy[d]);
        ret.push_back({movefrom, moveto});
        board.move(emptyPos[0] + dx[d], emptyPos[1] + dy[d]);
      }
    }
    return ret;
  }

  struct LineSolverInfo {
    vector<array<int, 2>> moves;
    int worst_route_move_count = 0;
    int worst_move_id = -1;
  };

  vector<P> string_to_route(const Board &board, string str) {
    vector<P> ret;
    auto pos = board.get_selected();
    for (char c : str) {
      auto d = dstr.find(c);
      P next_pos = {pos[0] + dx[d], pos[1] + dy[d]};
      ret.push_back(next_pos);
      pos = next_pos;
    }
    return ret;
  };

  LineSolverInfo process_line_without_edge(
      Board board, const vector<pair<P, int>> &targets) {
    LineSolverInfo ret;

    for (int idx = 0; idx < targets.size() - 2; idx++) {
      auto item = targets[idx];
      P pos = getPos(board, item.second);
      int aimingX = item.first[0];
      int aimingY = item.first[1];
      auto route = ultra_dijkstra(board, pos[0], pos[1], aimingX, aimingY);
      if (!is_valid_route(route)) {
        board.debug();
        assert(false);
      }
      int move_count = 0;
      for (auto way : route) {
        auto emptyPos = board.get_selected();
        board.lock(pos[0], pos[1]);
        auto route2 = dijkstra(board, emptyPos[0], emptyPos[1], way[0], way[1]);
        for (auto way2 : route2) {
          ret.moves.push_back(way2);
          board.move(way2[0], way2[1]);
        }
        move_count += route2.size();
        board.unlock(pos[0], pos[1]);
        ret.moves.push_back(pos);
        board.move(pos[0], pos[1]);
        pos = way;
      }
      if (ret.worst_route_move_count < move_count) {
        ret.worst_route_move_count = move_count;
        ret.worst_move_id = item.second;
      }
      board.lock(aimingX, aimingY);
    }
    return ret;
  }

  vector<P> process_edge_type_a(Board board,
                                const vector<pair<P, int>> &targets) {
    // 1 2 3 e a
    // * * * * b
    // からのRDする

    const auto a = *(targets.end() - 2);
    const auto b = *(targets.end() - 1);
    vector<P> ret;

    // aを持ってきて固定
    P a_pos = getPos(board, a.second);
    const int a_tox = a.first[0] + 1;
    const int a_toy = a.first[1];
    const auto a_route =
        ultra_dijkstra(board, a_pos[0], a_pos[1], a_tox, a_toy);
    for (auto way : a_route) {
      const auto ep = board.get_selected();
      board.lock(a_pos[0], a_pos[1]);
      auto route2 = dijkstra(board, ep[0], ep[1], way[0], way[1]);
      for (auto way2 : route2) {
        board.move(way2[0], way2[1]);
        ret.push_back(way2);
      }

      board.unlock(a_pos[0], a_pos[1]);
      board.move(a_pos[0], a_pos[1]);
      ret.push_back(a_pos);
      a_pos = way;
    }
    board.lock(a_tox, a_toy);

    // bを持ってきて固定
    P b_pos = getPos(board, b.second);
    int b_tox = b.first[0];
    int b_toy = b.first[1] + 1;
    const auto b_route =
        ultra_dijkstra(board, b_pos[0], b_pos[1], b_tox, b_toy);
    if (is_valid_route(b_route)) {
      for (auto way : b_route) {
        const auto ep = board.get_selected();
        board.lock(b_pos[0], b_pos[1]);
        auto route2 = dijkstra(board, ep[0], ep[1], way[0], way[1]);
        board.unlock(b_pos[0], b_pos[1]);
        for (auto way2 : route2) {
          board.move(way2[0], way2[1]);
          ret.push_back(way2);
        }
        board.move(b_pos[0], b_pos[1]);
        ret.push_back(b_pos);
        b_pos = way;
      }
    } else {
      // 持ってこれないパターンがあるので対策
      auto ep = board.get_selected();
      if (ep != a.first) {
        auto route2 = dijkstra(board, ep[0], ep[1], b_pos[0], b_pos[1]);
        if (!is_valid_route(route2)) {
          throw "error2";
        }
        for (auto way : route2) {
          board.move(way[0], way[1]);
          ret.push_back(way);
        }
      }
      auto route = string_to_route(board, "RDLDRUULDRDLUU");
      for (auto way : route) {
        board.move(way[0], way[1]);
        ret.push_back(way);
      }
    }
    board.lock(b_tox, b_toy);

    // eをもってくる
    const P e_pos = board.get_selected();
    const int e_tox = a.first[0];
    const int e_toy = a.first[1];
    auto route = dijkstra(board, e_pos[0], e_pos[1], e_tox, e_toy);
    if (!is_valid_route(route)) {
      board.debug();
      throw "error3";
    }
    for (auto way : route) {
      board.move(way[0], way[1]);
      ret.push_back(way);
    }
    for (auto way : string_to_route(board, "RD")) {
      board.move(way[0], way[1]);
      ret.push_back(way);
    }

    return ret;
  }

  vector<P> process_edge_type_b(Board board,
                                const vector<pair<P, int>> &targets) {
    // 1 2 3 a e
    // * * * b
    // からのLDする

    const auto b = *(targets.end() - 2);
    const auto a = *(targets.end() - 1);
    vector<P> ret;

    // aを持ってきて固定
    P a_pos = getPos(board, a.second);
    const int a_tox = a.first[0] - 1;
    const int a_toy = a.first[1];
    const auto a_route =
        ultra_dijkstra(board, a_pos[0], a_pos[1], a_tox, a_toy);
    for (auto way : a_route) {
      const auto ep = board.get_selected();
      board.lock(a_pos[0], a_pos[1]);
      auto route2 = dijkstra(board, ep[0], ep[1], way[0], way[1]);
      for (auto way2 : route2) {
        board.move(way2[0], way2[1]);
        ret.push_back(way2);
      }

      board.unlock(a_pos[0], a_pos[1]);
      board.move(a_pos[0], a_pos[1]);
      ret.push_back(a_pos);
      a_pos = way;
    }
    board.lock(a_tox, a_toy);

    // bを持ってきて固定
    P b_pos = getPos(board, b.second);
    int b_tox = b.first[0];
    int b_toy = b.first[1] + 1;
    const auto b_route =
        ultra_dijkstra(board, b_pos[0], b_pos[1], b_tox, b_toy);
    if (is_valid_route(b_route)) {
      for (auto way : b_route) {
        const auto ep = board.get_selected();
        board.lock(b_pos[0], b_pos[1]);
        auto route2 = dijkstra(board, ep[0], ep[1], way[0], way[1]);
        board.unlock(b_pos[0], b_pos[1]);
        for (auto way2 : route2) {
          board.move(way2[0], way2[1]);
          ret.push_back(way2);
        }
        board.move(b_pos[0], b_pos[1]);
        ret.push_back(b_pos);
        b_pos = way;
      }
    } else {
      // 持ってこれないパターンがあるので対策
      auto ep = board.get_selected();
      if (ep != a.first) {
        auto route2 = dijkstra(board, ep[0], ep[1], b_pos[0], b_pos[1]);
        if (!is_valid_route(route2)) {
          throw "error2";
        }
        for (auto way : route2) {
          board.move(way[0], way[1]);
          ret.push_back(way);
        }
      }
      auto route = string_to_route(board, "LDRDLUURDLDRUU");
      for (auto way : route) {
        board.move(way[0], way[1]);
        ret.push_back(way);
      }
    }
    board.lock(b_tox, b_toy);

    // eをもってくる
    const P e_pos = board.get_selected();
    const int e_tox = a.first[0];
    const int e_toy = a.first[1];
    auto route = dijkstra(board, e_pos[0], e_pos[1], e_tox, e_toy);
    if (!is_valid_route(route)) {
      board.debug();
      throw "error3";
    }
    for (auto way : route) {
      board.move(way[0], way[1]);
      ret.push_back(way);
    }
    for (auto way : string_to_route(board, "LD")) {
      board.move(way[0], way[1]);
      ret.push_back(way);
    }

    return ret;
  }

  LineSolverInfo line_solve(Board board, const vector<pair<P, int>> &targets) {
    LineSolverInfo ret;

    auto add_ans = [&ret, &board](const P &b) {
      const P a = board.get_selected();
      ret.moves.push_back({board.orgInd(a[0], a[1]), board.orgInd(b[0], b[1])});
    };

    auto ans_move = [&ret, &board, &add_ans](const P &b) {
      add_ans(b);
      board.move(b[0], b[1]);
    };

    // 端っこ２つ以外を埋める
    auto line_ans = process_line_without_edge(board, targets);

    for (auto move : line_ans.moves) {
      ans_move(move);
    }

    for (int i = 0; i < targets.size() - 2; ++i) {
      const auto &p = targets[i].first;
      board.lock(p[0], p[1]);
    }

    if (ret.worst_route_move_count < line_ans.worst_route_move_count) {
      ret.worst_route_move_count = line_ans.worst_route_move_count;
      ret.worst_move_id = line_ans.worst_move_id;
    }

    // 端っこ２つを埋める
    auto edge_ans = process_edge_type_a(board, targets);

    auto edge_ans_b = process_edge_type_b(board, targets);

    if (edge_ans_b.size() < edge_ans.size()) {
      edge_ans = edge_ans_b;
    }

    for (auto move : edge_ans) {
      ans_move(move);
    }

    for (auto i = targets.size() - 2; i < targets.size(); ++i) {
      const auto &p = targets[i].first;
      board.lock(p[0], p[1]);
    }

    return ret;
  }

  struct GreedySolverInfo {
    LineSolverInfo worst_line_ans;
    vector<int> line_fixed_step;
  };

  bool fileExists(int w, int h, int k, string name = "") {
    stringstream filename;
    filename << "./database/";
    if (name == "") {
      filename << (char)(w < 10 ? '0' + w : 'A' + w - 10);
      filename << (char)(h < 10 ? '0' + h : 'A' + h - 10);
      filename << k;
    } else {
      filename << name;
    }

    FILE *fp;
    if ((fp = fopen(filename.str().c_str(), "rb")) == NULL) {
      return false;
    } else {
      fclose(fp);
      return true;
    }
  }

  vector<P> tableSearchMain(Board board, vector<P> from, P off, int w, int h,
                            string name = "") {
    int sx = board.get_selected()[0];
    int sy = board.get_selected()[1];
    stringstream filename;
    filename << "./database/";
    if (name == "") {
      filename << (char)(w < 10 ? '0' + w : 'A' + w - 10);
      filename << (char)(h < 10 ? '0' + h : 'A' + h - 10);
      filename << from.size();
    } else {
      filename << name;
    }
    FILE *fp;
    if ((fp = fopen(filename.str().c_str(), "rb")) == NULL) {
      return {{-1, -1}};
    }
    for (int i = 0; i < from.size(); i++) {
      from[i][0] -= off[0];
      from[i][1] -= off[1];
    }
    sx -= off[0];
    sy -= off[1];

    auto to1d = [&](P p) -> int { return p[1] * w + p[0]; };
    auto enc = [&](P x, vector<P> y) -> int {
      int res = to1d(x);
      int sz = w * h;
      for (int i = 0; i < y.size(); i++) {
        res = res * sz + to1d(y[i]);
      }
      return res;
    };
    auto get = [fp](int pos) -> char {
      char c;
      fseek(fp, pos, SEEK_SET);
      fread(&c, 1, 1, fp);
      return c;
    };
    P p = P{sx, sy};
    vector<P> result;
    char dir;
    if (get(enc(p, from)) == '-') {
      fclose(fp);
      return {{-1, -1}};
    }

    while ((dir = get(enc(p, from))) != '4') {
      int i = dir - '0';
      if (i < 0 || i > 4) {
        assert(0);  //不正な呼び出し
      }
      P next{p[0] + dx[i], p[1] + dy[i]};
      for (int j = 0; j < from.size(); j++) {
        if (from[j] == next) from[j] = p;
      }
      int a = board.orgInd(p[0] + off[0], p[1] + off[1]);
      int b = board.orgInd(next[0] + off[0], next[1] + off[1]);
      p = next;
      result.push_back({a, b});
    }
    fclose(fp);
    return result;
  }

  vector<array<int, 2>> tableSolve_all(Board board, int w, int h) {
    P off = board.offset();
    P emp = board.imgPos(board.ele(board.get_selected()));
    vector<P> getting_positions;
    for (int i = 0; i < board.height(); i++) {
      for (int j = 0; j < board.width(); j++) {
        if (board.is_locked(j, i) == 0 &&
            P{j, i} != emp) {  // too many items must be trashes.
          P nowPos = board.where(board.orgInd(j, i));
          getting_positions.push_back(nowPos);
        }
      }
    }
    return tableSearchMain(board, getting_positions, off, w, h,
                           itos(w) + itos(h) + "special" +
                               itos((emp[1] - off[1]) * w + emp[0] - off[0]));
  }

  vector<P> nearDijkstra(const Board &board, int sx, int sy, Focus focus) {
    if (board.is_locked(sx, sy)) return {{-1, -1}};  // error
    const int W = board.width();
    const int H = board.height();

    struct Node {
      P p;
      Board b;
      int cost;
      Node(const Node &o) : p(o.p), b(o.b), cost(o.cost) {}
      Node(Board b, P p, int cost) : p(p), b(b), cost(cost) {}
      bool operator<(const Node &x) const { return cost > x.cost; }
    };

    const auto to1d = [](P p) -> int { return p[1] * 16 + p[0]; };
    const auto to2d = [](int v) -> P { return {v % 16, v / 16}; };
    const auto ok = [W, H](const Board &b, const P &p) -> bool {
      return !(p[0] < 0 || p[0] >= W || p[1] < 0 || p[1] >= H ||
               b.is_locked(p[0], p[1]));
    };
    const auto swapEffect = [](const Board &board, const P &a, const P &b)
                                -> int {
      // 空白セルが a から b に移動したとき,
      // ともなって動いたセルのコストを計算する
      // boardは移動前の状態であること
      const int OW = board.orgWidth();
      const int num = board.ele(b[0], b[1]);
      const int prev = board.orgInd(b[0], b[1]);
      const int next = board.orgInd(a[0], a[1]);
      int dist_prev =
          board.calc_move_distance(num % OW - prev % OW, num / OW - prev / OW);
      int dist_next =
          board.calc_move_distance(num % OW - next % OW, num / OW - next / OW);
      return 4 + dist_next - dist_prev;
    };

    struct SubDijkstraResult {
      vector<P> moves;
      int cost;
    };
    auto sub_dijkstra = [to1d, to2d, ok, swapEffect](const Board &board, int sx,
                                                     int sy, int gx, int gy)
                            -> SubDijkstraResult {
      array<int, 16 * 16> shortestCost;
      shortestCost.fill(1e9);
      array<int, 16 * 16> prev;
      prev.fill(-1);
      struct Node {
        P p;
        int cost;
        Node(P p, int cost) : p(p), cost(cost) {}
        bool operator<(const Node &x) const { return cost > x.cost; }
      };
      priority_queue<Node> Q;
      Q.emplace(P{sx, sy}, 0);
      shortestCost[to1d({sx, sy})] = 0;

      while (Q.size()) {
        Node q = Q.top();
        Q.pop();
        if (shortestCost[to1d(q.p)] != q.cost) continue;
        if (q.p == P{gx, gy}) {
          SubDijkstraResult res;
          res.cost = q.cost;
          while (q.p != P{sx, sy}) {
            res.moves.push_back(q.p);
            q.p = to2d(prev[to1d(q.p)]);
          }
          reverse(res.moves.begin(), res.moves.end());
          return res;
        }
        for (int i = 0; i < 4; i++) {
          const P next{q.p[0] + dx[i], q.p[1] + dy[i]};
          if (!ok(board, next)) continue;
          int nextCost = q.cost + 1 + swapEffect(board, q.p, next);
          if (shortestCost[to1d(next)] <= nextCost) continue;
          shortestCost[to1d(next)] = nextCost;
          prev[to1d(next)] = to1d(q.p);
          Q.emplace(next, nextCost);
        }
      }

      SubDijkstraResult bad;
      bad.cost = -1;
      return bad;
    };

    array<int, 16 * 16> shortestCost;
    shortestCost.fill(1e6);
    array<int, 16 * 16> prev;
    prev.fill(-1);
    priority_queue<Node> Q;
    Node node(board, {sx, sy}, 0);
    shortestCost[to1d({sx, sy})] = 0;
    Q.push(node);

    while (Q.size()) {
      Node q = Q.top();
      Q.pop();
      if (shortestCost[to1d(q.p)] != q.cost) continue;
      if (focus.in(q.p)) {
        vector<P> res = {};
        while (q.p != P{sx, sy}) {
          res.push_back(q.p);
          q.p = to2d(prev[to1d(q.p)]);
        }
        reverse(res.begin(), res.end());
        return res;
      }

      q.b.lock(q.p[0], q.p[1]);  // 移動させたいピースの位置を固定
      for (int d = 0; d < 4; d++) {
        const P next_pos{q.p[0] + dx[d],
                         q.p[1] + dy[d]};  // 移動させたいピースの近傍
        if (!ok(q.b, next_pos)) continue;
        if (shortestCost[to1d(next_pos)] <= q.cost) continue;

        // 現在の空白マス -> 移動させたいピースの近傍へダイクストラ
        const P empty_pos = q.b.get_selected();
        auto submove = sub_dijkstra(q.b, empty_pos[0], empty_pos[1],
                                    next_pos[0], next_pos[1]);
        if (submove.cost != -1) {
          Node next_node = q;
          next_node.cost += submove.cost;
          for (const P &p : submove.moves) {
            next_node.b.move(p[0], p[1]);
          }

          // 移動させたいピースの近傍にいるはず
          next_node.cost += 1 + swapEffect(next_node.b, next_pos, q.p);

          // 移動させたいピースを１マス移動させたが
          // それよりも短いコストで同じ位置まで移動できる経路が存在した場合は無視
          if (shortestCost[to1d(next_pos)] <= next_node.cost) continue;
          shortestCost[to1d(next_pos)] = next_node.cost;

          // 空白セルを移動させたいピースと入れ替え、移動させたいピースを１マス移動する
          next_node.b.unlock(q.p[0], q.p[1]);
          next_node.b.move(q.p[0], q.p[1]);
          next_node.p = next_pos;
          prev[to1d(next_node.p)] = to1d(q.p);
          Q.emplace(next_node);
        }
      }
    }
    return {{-1, -1}};  // failed
  };

  vector<P> near(Board board, vector<pair<P, int>> targets, Focus focus,
                 LineSolverInfo &info) {
    vector<P> res;
    for (auto target : targets) {
      target.first = board.where(target.second);
      if (focus.in(target.first)) {
        board.lock(target.first);
      } else {
        int id = board.ele(target.first);
        auto way = nearDo(board, target.first[0], target.first[1], focus);
        if (!is_valid_route(way)) return {{-1, -1}};

        if (info.worst_route_move_count < way.size()) {
          info.worst_route_move_count = (int)way.size();
          info.worst_move_id = target.second;
        }

        res.insert(res.end(), way.begin(), way.end());
        for (auto &move : way) {
          board.move(move[0], move[1]);
        }

        board.lock(board.where(id));
      }
    }

    auto emptyPos = board.get_selected();
    auto way = nearMiniDijkstra(board, emptyPos[0], emptyPos[1], focus);
    if (!is_valid_route(way)) return {{-1, -1}};
    res.insert(res.end(), way.begin(), way.end());
    for (auto &move : way) {
      board.move(move[0], move[1]);
    }
    return res;
  }

  vector<pair<P, int>> fix(Board board, vector<pair<P, int>> from) {
    for (int i = 0; i < from.size(); i++)
      from[i].first = board.where(from[i].second);
    return from;
  }
  LineSolverInfo table_oneLine(Board board, vector<pair<P, int>> targets,
                               Focus focus) {
    Board clone_board = board;
    LineSolverInfo ret;

    auto add_ans = [&ret, &board](const P &b) {
      const P a = board.get_selected();
      ret.moves.push_back({board.orgInd(a[0], a[1]), board.orgInd(b[0], b[1])});
    };

    for (int k = 0; k < targets.size();) {
      targets = fix(board, targets);
      vector<pair<P, int>> subtarget;
      for (int j = k; j < targets.size(); j++) subtarget.push_back(targets[j]);

      while (subtarget.size()) {
        int tryW = -1, tryH = -1;
        for (int i = min(5, focus.height); i >= 3; i--)
          for (int j = focus.width; j >= 1; j--)
            if (tryW == -1 && fileExists(j, i, (int)subtarget.size())) {
              tryW = j;
              tryH = i;
            }
        if (tryW == -1) {
          subtarget.pop_back();
        } else {
          int extend = 0;
          int maximum = (int)subtarget.size();
          for (int i = 0; tryW + i <= focus.width; i++)
            if (fileExists(tryW + i, tryH, tryW + i)) {
              extend = max(extend, i);
              maximum = max(tryW + i, maximum);
            }
          tryW += extend;
          int memoMaxLimitConsider = maximum;
          int startX = focus.offset[0] + k;
          while (startX + memoMaxLimitConsider >
                     focus.width + focus.offset[0] &&
                 startX > focus.offset[0]) {
            startX--;
          }
          while (startX + tryW > focus.width + focus.offset[0]) {
            tryW--;
          }
          vector<pair<P, int>> subtarget2;
          for (int i = 0; i < min(tryW, memoMaxLimitConsider); i++) {
            subtarget2.push_back(targets[startX + i - focus.offset[0]]);

            while (startX + i - focus.offset[0] < 0 ||
                   startX + i - focus.offset[0] >=
                       focus.offset[0] + focus.width) {
            }
          }
          subtarget2 = fix(board, subtarget2);

          for (int j = focus.offset[0]; j < startX; j++) {
            board.lock(j, focus.offset[1]);
          }

          Focus tryFocus = Focus({startX, focus.offset[1]}, tryW, tryH);
          vector<P> nearway = near(board, subtarget2, tryFocus, ret);
          if (!is_valid_route(nearway)) {
            ret.moves = {{-1, -1}};
            return ret;
          }

          for (auto &move : nearway) {
            P to = move;
            add_ans(to);
            board.move(to[0], to[1]);
          }

          subtarget2 = fix(board, subtarget2);
          vector<P> tmp;
          for (int i = 0; i < subtarget2.size(); i++)
            tmp.push_back(subtarget2[i].first);

          vector<P> mainway = tableSearchMain(board, tmp, tryFocus.offset,
                                              tryFocus.width, tryFocus.height);

          if (!is_valid_route(mainway)) {
            ret.moves = {{-1, -1}};
            return ret;
          }
          for (auto &move : mainway) {
            P to = board.imgPos(move[1]);
            add_ans(to);
            board.move(to[0], to[1]);
          }
          subtarget2 = fix(board, subtarget2);

          for (int j = focus.offset[0]; j < startX; j++) {
            board.unlock(j, focus.offset[1]);
          }
          break;
        }
      }

      // DATABASE IS NOT FOUND
      if (subtarget.size() == 0) {
        ret.moves = {{-1, -1}};
        return ret;
      }
      k += subtarget.size();
    }

    for (auto i = 0; i < targets.size(); ++i) {
      const auto &p = board.where(targets[i].second);
      board.lock(p[0], p[1]);
    }
    return ret;
  }

  GreedySolverInfo greedy2(Board current, int sx, int sy) {
    current.select(sx, sy);
    int wid = current.width(), hei = current.height();

    auto local_rotate = [&]() {
      swap(hei, wid);
      current.rotate();
    };

    GreedySolverInfo ret;
    ret.worst_line_ans.worst_move_id = -1;

    while (1) {
      if (wid > hei) local_rotate();
      Board board = current;
      if (wid <= 3 && hei <= 3) {
        auto moves = tableSolve_all(board, wid, hei);
        if (is_valid_route(moves)) {
          for (auto &move : moves) {
            P to = current.imgPos(move[1]);
            current.move(to[0], to[1]);
            answer.push(move[0], move[1]);
          }
          break;
        }
      }

      if (wid == 2 && hei == 2) {
        auto moves = get_2x2_finalize_move(board);
        for (auto &move : moves) {
          P to = current.imgPos(move[1]);
          current.move(to[0], to[1]);
          answer.push(move[0], move[1]);
        }

        break;
      }

      //今回埋めたいセルが今どこにあるかと、どこに持っていくかのリスト
      auto targets = get_next_target_positions(board);
      bool avoid = targets_contain_selected_pos(board, targets);
      bool completed = targets_are_already_completed(board, targets);

      //やめたほうがいいらしい場合は次
      if (avoid) {
        local_rotate();
        continue;
      }

      if (!completed) {
        // 1行埋めるソルバに食わせる
        auto line_ans =
            table_oneLine(board, targets, Focus(board.offset(), wid, hei));
        if (!is_valid_route(line_ans.moves)) {
          line_ans = line_solve(board, targets);
        }
        if (ret.worst_line_ans.worst_move_id == -1 ||
            ret.worst_line_ans.worst_route_move_count <
                line_ans.worst_route_move_count) {
          ret.worst_line_ans = line_ans;
        }

        //ソルバの解答をcurrentに反映
        for (auto &move : line_ans.moves) {
          P to = current.imgPos(move[1]);
          current.move(to[0], to[1]);
          answer.push(move[0], move[1]);
        }
      }

      //埋まった行を固定する
      for (auto &fixed : targets) {
        P fixed_pos = current.imgPos(fixed.second);
        current.lock(fixed_pos[0], fixed_pos[1]);
      }

      hei--;
    }
    return ret;
  }
};
