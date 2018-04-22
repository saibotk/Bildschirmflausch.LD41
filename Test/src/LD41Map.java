import java.awt.Color;
import java.awt.Dimension;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.Point;
import java.awt.Rectangle;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.util.Comparator;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;
import java.util.Set;
import javax.swing.JFrame;
import javax.swing.JPanel;
import org.joml.Vector2i;
import javafx.util.Pair;

public class LD41Map {

	static final int TUNNEL_THICKNESS = 4;

	public static class Vertex {

		public final Room	r;
		public float		value;

		public Vertex(Room r) {
			this.r = r;
			value = Float.POSITIVE_INFINITY;
		}
	}

	public static class Edge {

		public final Vertex	r1, r2;
		public double		dist;

		public Edge(Vertex r1, Vertex r2) {
			this.r1 = r1;
			this.r2 = r2;
			dist = r1.r.distance(r2.r);
		}
	}

	public static class Room {

		Rectangle				bounds		= new Rectangle();
		Map<Vector2i, TileType>	tiles		= new HashMap<>();
		Set<Vector2i>			doorsUp		= new HashSet<>();
		Set<Vector2i>			doorsDown	= new HashSet<>();
		Set<Vector2i>			doorsLeft	= new HashSet<>();
		Set<Vector2i>			doorsRight	= new HashSet<>();

		public float distance(Room r) {
			return Math.abs(getCenter().x - r.getCenter().x) + Math.abs(getCenter().y - r.getCenter().y);
		}

		public Point getCenter() {
			return new Point((int) bounds.getCenterX(), (int) bounds.getCenterY());
		}

		public Set<Vector2i> allDoors() {
			Set<Vector2i> ret = new HashSet<>();
			ret.addAll(doorsUp);
			ret.addAll(doorsDown);
			ret.addAll(doorsLeft);
			ret.addAll(doorsRight);
			return ret;
		}
	}

	public static enum TileType {
		GROUND, WALL, DOOR;
	}

	public static void main(String[] args) {
		Set<Room> rooms = new HashSet<>();
		rooms.addAll(generate().getKey());

		{
			JFrame frame = new JFrame();
			frame.setDefaultCloseOperation(3);
			JPanel panel = new JPanel() {

				private static final long serialVersionUID = 5954622107064881267L;

				@Override
				public void paintComponent(Graphics h) {
					super.paintComponent(h);
					Pair<Set<Room>, Pair<Vertex, Set<Edge>>> graph = generate();
					Graphics2D g = (Graphics2D) h;
					g.translate(getWidth() / 2, getHeight() / 2);
					g.scale(4, 4);
					g.setColor(new Color(1f - (float) Math.random() * 0.3f, 0f, 0f, 0.8f));
					for (Room room : graph.getKey()) {
						room.tiles.forEach((v, type) -> {
							if (type == TileType.GROUND)
								g.fillRect(v.x, v.y, 1, 1);
						});
					}
					g.setColor(new Color(0f, 0f, 1f, 0.8f));
					for (Room room : graph.getKey()) {
						room.tiles.forEach((v, type) -> {
							if (type == TileType.WALL)
								g.fillRect(v.x, v.y, 1, 1);
						});
					}
					g.setColor(new Color(0f, 1f, 0f, 0.8f));
					for (Room room : graph.getKey()) {
						room.tiles.forEach((v, type) -> {
							if (type == TileType.DOOR)
								g.fillRect(v.x, v.y, 1, 1);
						});
					}
				}
			};
			frame.add(panel);
			frame.setPreferredSize(new Dimension(800, 500));
			frame.pack();
			frame.setVisible(true);
			frame.repaint();
			frame.setExtendedState(6);

			frame.addKeyListener(new KeyListener() {

				@Override
				public void keyTyped(KeyEvent e) {
					rooms.clear();
					Pair<Set<Room>, Pair<Vertex, Set<Edge>>> graph = generate();
					rooms.addAll(graph.getKey());
					frame.repaint();
				}

				@Override
				public void keyReleased(KeyEvent e) {
				}

				@Override
				public void keyPressed(KeyEvent e) {
				}
			});
		}
	}

	public static Pair<Set<Room>, Pair<Vertex, Set<Edge>>> generate() {
		int minRoomSize = 50;
		Set<Room> rooms = new HashSet<>();
		for (int i = 0; i < 7 + (int) (Math.random() * 4); i++) {
			Room room = new Room();
			room.bounds.width = ((15 + (int) (Math.random() * 20)) / 2) * 2;
			room.bounds.height = ((15 + (int) (Math.random() * 20)) / 2) * 2;
			rooms.add(room);
		}

		outest: while (true) {
			for (Room r1 : rooms) {
				for (Room r2 : rooms) {
					if (r1 == r2)
						continue;
					Vector2i p1 = new Vector2i(r1.bounds.x + r1.bounds.width / 2, r1.bounds.y + r1.bounds.height / 2);
					Vector2i p2 = new Vector2i(r2.bounds.x + r2.bounds.width / 2, r2.bounds.y + r2.bounds.height / 2);
					if (p1.distanceSquared(p2) < 2 * minRoomSize * minRoomSize + 2) {
						r2.bounds.x += (int) ((Math.random() - 0.5) * 5);
						r2.bounds.y += (int) ((Math.random() - 0.5) * 2.5);
						continue outest;
					}
				}
			}
			break;
		}

		Set<Vertex> Q = new HashSet<>();
		for (Room r : rooms)
			Q.add(new Vertex(r));
		Vertex root = Q.stream().min(Comparator.comparing(v -> v.r.bounds.x)).get();
		root.value = 0;

		Set<Edge> E = new HashSet<>();
		Set<Edge> G = new HashSet<>();
		Set<Vertex> F = new HashSet<>();
		for (Vertex r1 : Q) {
			outer: for (Vertex r2 : Q) {
				if (r1 == r2)
					continue outer;
				for (Edge e : E)
					if (e.r2 == r1 && e.r1 == r2)
						continue outer;
				E.add(new Edge(r1, r2));
			}
		}
		F.add(root);
		Q.remove(root);

		while (!Q.isEmpty()) {
			Edge start = E.stream()
					.filter(e -> (F.contains(e.r1) ^ F.contains(e.r2)))
					.min(Comparator.comparing(e -> e.dist)).get();
			Q.remove(start.r2);
			Q.remove(start.r1);
			F.add(start.r2);
			F.add(start.r1);
			E.remove(start);
			G.add(start);
			if (start.r1.value < start.r2.value) {
				start.r2.value = (float) (start.r1.value + start.dist);
			} else {
				start.r1.value = (float) (start.r2.value + start.dist);
			}
		}

		// G list of edges
		// rooms list of rooms

		Set<Room> rooms2 = new HashSet<>();

		for (Edge ed : G) {
			// horizontal
			float diff1 = ed.r1.r.bounds.y - ed.r2.r.bounds.y - ed.r2.r.bounds.height + TUNNEL_THICKNESS;
			float diff2 = ed.r2.r.bounds.y - ed.r1.r.bounds.y - ed.r1.r.bounds.height + TUNNEL_THICKNESS;

			// vertical
			float diff3 = ed.r1.r.bounds.x - ed.r2.r.bounds.x - ed.r2.r.bounds.width + TUNNEL_THICKNESS;
			float diff4 = ed.r2.r.bounds.x - ed.r1.r.bounds.x - ed.r1.r.bounds.width + TUNNEL_THICKNESS;

			if (diff1 < 0 && diff2 < 0) {
				addStraightHorizontal(rooms2, ed);
			} else if (diff3 < 0 && diff4 < 0) {
				addStraightVertical(rooms2, ed);
			} else
				addCurve(rooms2, ed);
		}

		Room path = new Room();
		for (Room r : rooms2) {
			for (int x1 = r.bounds.x; x1 < r.bounds.x + r.bounds.width; x1++)
				for (int y1 = r.bounds.y; y1 < r.bounds.y + r.bounds.height; y1++) {
					path.tiles.put(new Vector2i(x1, y1), TileType.GROUND);
					for (int x2 = x1 - 1; x2 <= x1 + 1; x2++)
						for (int y2 = y1 - 1; y2 <= y1 + 1; y2++) {
							path.tiles.putIfAbsent(new Vector2i(x2, y2), TileType.WALL);
						}
				}
			for (Vector2i v : r.allDoors())
				r.tiles.put(v, TileType.DOOR);
		}
		for (Room r : rooms) {
			for (int x1 = r.bounds.x; x1 < r.bounds.x + r.bounds.width; x1++)
				for (int y1 = r.bounds.y; y1 < r.bounds.y + r.bounds.height; y1++) {
					r.tiles.put(new Vector2i(x1, y1), TileType.WALL);
				}
			for (int x1 = r.bounds.x + 1; x1 < r.bounds.x + r.bounds.width - 1; x1++)
				for (int y1 = r.bounds.y + 1; y1 < r.bounds.y + r.bounds.height - 1; y1++) {
					r.tiles.put(new Vector2i(x1, y1), TileType.GROUND);
				}
			for (Vector2i v : r.allDoors())
				r.tiles.put(v, TileType.DOOR);
		}
		rooms.add(path);
		return new Pair<>(rooms, new Pair<>(root, G));
	}

	public static void addStraightHorizontal(Set<Room> rooms, Edge ed) {
		Room righter = ed.r1.r.getCenter().x > ed.r2.r.getCenter().x ? ed.r1.r : ed.r2.r;
		Room lefter = ed.r1.r.getCenter().x > ed.r2.r.getCenter().x ? ed.r2.r : ed.r1.r;
		Room tunnel = new Room();
		int minX = Math.min(ed.r1.r.bounds.x + ed.r1.r.bounds.width, ed.r2.r.bounds.x + ed.r2.r.bounds.width);
		int minY = Math.max(ed.r1.r.bounds.y, ed.r2.r.bounds.y);
		int maxX = Math.max(ed.r1.r.bounds.x, ed.r2.r.bounds.x);
		int maxY = Math.min(ed.r1.r.bounds.y + ed.r1.r.bounds.height, ed.r2.r.bounds.y + ed.r2.r.bounds.height);
		tunnel.bounds.x = minX;
		tunnel.bounds.y = (minY + maxY) / 2 - TUNNEL_THICKNESS / 2;
		tunnel.bounds.width = (maxX - minX);
		tunnel.bounds.height = TUNNEL_THICKNESS;

		rooms.add(tunnel);

		for (int i = 0; i < TUNNEL_THICKNESS; i++) {
			lefter.doorsRight.add(new Vector2i(tunnel.bounds.x - 1, tunnel.bounds.y + i));
			righter.doorsLeft.add(new Vector2i(tunnel.bounds.x + tunnel.bounds.width, tunnel.bounds.y + i));
		}
	}

	public static void addStraightVertical(Set<Room> rooms, Edge ed) {
		Room higher = ed.r1.r.getCenter().y > ed.r2.r.getCenter().y ? ed.r1.r : ed.r2.r;
		Room lower = ed.r1.r.getCenter().y > ed.r2.r.getCenter().y ? ed.r2.r : ed.r1.r;
		Room tunnel = new Room();
		int minX = Math.max(ed.r1.r.bounds.x, ed.r2.r.bounds.x);
		int minY = Math.min(ed.r1.r.bounds.y + ed.r1.r.bounds.height, ed.r2.r.bounds.y + ed.r2.r.bounds.height);
		int maxX = Math.min(ed.r1.r.bounds.x + ed.r1.r.bounds.width, ed.r2.r.bounds.x + ed.r2.r.bounds.width);
		int maxY = Math.max(ed.r1.r.bounds.y, ed.r2.r.bounds.y);
		tunnel.bounds.x = (minX + maxX) / 2 - TUNNEL_THICKNESS / 2;
		tunnel.bounds.y = minY;
		tunnel.bounds.width = TUNNEL_THICKNESS;
		tunnel.bounds.height = (maxY - minY);

		rooms.add(tunnel);

		for (int i = 0; i < TUNNEL_THICKNESS; i++) {
			lower.doorsUp.add(new Vector2i(tunnel.bounds.x + i, tunnel.bounds.y + tunnel.bounds.height));
			higher.doorsDown.add(new Vector2i(tunnel.bounds.x + i, tunnel.bounds.y - 1));
		}
	}

	public static void addCurve(Set<Room> rooms, Edge ed) {
		Room higher = ed.r1.r.getCenter().y > ed.r2.r.getCenter().y ? ed.r1.r : ed.r2.r;
		Room lower = ed.r1.r.getCenter().y > ed.r2.r.getCenter().y ? ed.r2.r : ed.r1.r;
		Room righter = ed.r1.r.getCenter().x > ed.r2.r.getCenter().x ? ed.r1.r : ed.r2.r;
		Room lefter = ed.r1.r.getCenter().x > ed.r2.r.getCenter().x ? ed.r2.r : ed.r1.r;

		Rectangle r = new Rectangle(lefter.getCenter().x, lower.getCenter().y, righter.getCenter().x - lefter.getCenter().x, higher.getCenter().y - lower.getCenter().y);

		Room verticalLefter = new Room();
		verticalLefter.bounds.x = r.x - TUNNEL_THICKNESS / 2;
		verticalLefter.bounds.y = r.y - TUNNEL_THICKNESS / 2;
		verticalLefter.bounds.width = TUNNEL_THICKNESS;
		verticalLefter.bounds.height = r.height + TUNNEL_THICKNESS;

		Room horizontalLower = new Room();
		horizontalLower.bounds.x = r.x - TUNNEL_THICKNESS / 2;
		horizontalLower.bounds.y = r.y - TUNNEL_THICKNESS / 2;
		horizontalLower.bounds.width = r.width + TUNNEL_THICKNESS;
		horizontalLower.bounds.height = TUNNEL_THICKNESS;

		Room verticalRighter = new Room();
		verticalRighter.bounds.x = r.x + r.width - TUNNEL_THICKNESS / 2;
		verticalRighter.bounds.y = r.y - TUNNEL_THICKNESS / 2;
		verticalRighter.bounds.width = TUNNEL_THICKNESS;
		verticalRighter.bounds.height = r.height + TUNNEL_THICKNESS;

		Room horizontalHigher = new Room();
		horizontalHigher.bounds.x = r.x - TUNNEL_THICKNESS / 2;
		horizontalHigher.bounds.y = r.y + r.height - TUNNEL_THICKNESS / 2;
		horizontalHigher.bounds.width = r.width + TUNNEL_THICKNESS;
		horizontalHigher.bounds.height = TUNNEL_THICKNESS;

		if (lower == lefter) {
			horizontalLower.bounds.x = r.x + lower.bounds.width / 2;
			horizontalLower.bounds.width = r.width - lower.bounds.width / 2 + TUNNEL_THICKNESS / 2;
			horizontalHigher.bounds.width = r.width - higher.bounds.width / 2 + TUNNEL_THICKNESS / 2;

			verticalLefter.bounds.y = r.y + lower.bounds.height / 2;
			verticalLefter.bounds.height = r.height - lower.bounds.height / 2 + TUNNEL_THICKNESS / 2;
			verticalRighter.bounds.height = r.height - higher.bounds.height / 2 + TUNNEL_THICKNESS / 2;
		}
		if (lower == righter) {
			horizontalHigher.bounds.x = r.x + higher.bounds.width / 2;
			horizontalHigher.bounds.width = r.width - higher.bounds.width / 2 + TUNNEL_THICKNESS / 2;
			horizontalLower.bounds.width = r.width - lower.bounds.width / 2 + TUNNEL_THICKNESS / 2;

			verticalRighter.bounds.y = r.y + lower.bounds.height / 2;
			verticalRighter.bounds.height = r.height - lower.bounds.height / 2 + TUNNEL_THICKNESS / 2;
			verticalLefter.bounds.height = r.height - higher.bounds.height / 2 + TUNNEL_THICKNESS / 2;
		}

		boolean flip = Math.random() > 0.5;
		boolean diffX = ed.r2.r.getCenter().x - ed.r1.r.getCenter().x > 0;
		boolean diffY = ed.r2.r.getCenter().y - ed.r1.r.getCenter().y > 0;
		boolean addHorizontal1 = false, addHorizontal2 = false, addVertical1 = false, addVertical2 = false;
		if (diffX && diffY) {
			if (flip) {
				addVertical1 = true;
				addHorizontal2 = true;
			} else {
				addVertical2 = true;
				addHorizontal1 = true;
			}
		} else if (diffX && !diffY) {
			if (flip) {
				addVertical2 = true;
				addHorizontal2 = true;
			} else {
				addVertical1 = true;
				addHorizontal1 = true;
			}
		} else if (!diffX && diffY) {
			if (flip) {
				addVertical1 = true;
				addHorizontal1 = true;
			} else {
				addVertical2 = true;
				addHorizontal2 = true;
			}
		} else if (!diffX && !diffY) {
			if (flip) {
				addVertical2 = true;
				addHorizontal1 = true;
			} else {
				addVertical1 = true;
				addHorizontal2 = true;
			}
		}
		if (addHorizontal1) {
			rooms.add(horizontalLower);
			if (lower == lefter)
				for (int i = 0; i < TUNNEL_THICKNESS; i++) {
					lower.doorsRight.add(new Vector2i(horizontalLower.bounds.x - 1, horizontalLower.bounds.y + i));
				}
			else
				for (int i = 0; i < TUNNEL_THICKNESS; i++) {
					lower.doorsLeft.add(new Vector2i(horizontalLower.bounds.x + horizontalLower.bounds.width, horizontalLower.bounds.y + i));
				}
		}
		if (addHorizontal2) {
			rooms.add(horizontalHigher);
			if (lower == righter)
				for (int i = 0; i < TUNNEL_THICKNESS; i++) {
					higher.doorsRight.add(new Vector2i(horizontalHigher.bounds.x - 1, horizontalHigher.bounds.y + i));
				}
			else
				for (int i = 0; i < TUNNEL_THICKNESS; i++) {
					higher.doorsLeft.add(new Vector2i(horizontalHigher.bounds.x + horizontalHigher.bounds.width, horizontalHigher.bounds.y + i));
				}
		}
		if (addVertical1) {
			rooms.add(verticalLefter);
			if (lower == lefter)
				for (int i = 0; i < TUNNEL_THICKNESS; i++) {
					lower.doorsDown.add(new Vector2i(verticalLefter.bounds.x + i, verticalLefter.bounds.y - 1));
				}
			else
				for (int i = 0; i < TUNNEL_THICKNESS; i++) {
					lower.doorsUp.add(new Vector2i(verticalLefter.bounds.x + i, verticalLefter.bounds.y + verticalLefter.bounds.height));
				}
		}
		if (addVertical2) {
			rooms.add(verticalRighter);
			if (lower == righter)
				for (int i = 0; i < TUNNEL_THICKNESS; i++) {
					higher.doorsDown.add(new Vector2i(verticalRighter.bounds.x + i, verticalRighter.bounds.y - 1));
				}
			else
				for (int i = 0; i < TUNNEL_THICKNESS; i++) {
					higher.doorsUp.add(new Vector2i(verticalRighter.bounds.x + i, verticalRighter.bounds.y + verticalRighter.bounds.height));
				}
		}
	}
}